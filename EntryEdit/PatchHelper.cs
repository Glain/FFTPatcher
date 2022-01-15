using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PatcherLib.Datatypes;
using PatcherLib.Helpers;
using PatcherLib.Iso;
using PatcherLib.Utilities;

namespace EntryEdit
{
    public class PatchHelper
    {
        public static EntryData GetEntryDataFromPatchFile(string filepath, DataHelper dataHelper)
        {
            byte[] bytesBattleConditionals, bytesWorldConditionals, bytesEvents;
            EntryBytes defaultEntryBytes = dataHelper.LoadDefaultEntryBytes();

            using (ICSharpCode.SharpZipLib.Zip.ZipFile file = new ICSharpCode.SharpZipLib.Zip.ZipFile(filepath))
            {
                bytesBattleConditionals = PatcherLib.Utilities.Utilities.GetZipEntry(file, DataHelper.EntryNameBattleConditionals, false) ?? defaultEntryBytes.BattleConditionals;
                bytesWorldConditionals = PatcherLib.Utilities.Utilities.GetZipEntry(file, DataHelper.EntryNameWorldConditionals, false) ?? defaultEntryBytes.WorldConditionals;
                bytesEvents = PatcherLib.Utilities.Utilities.GetZipEntry(file, DataHelper.EntryNameEvents, false) ?? defaultEntryBytes.Events;
            }

            return dataHelper.LoadEntryDataFromBytes(bytesBattleConditionals, bytesWorldConditionals, bytesEvents);
        }

        public static void PatchISO(EntryData entryData, string filepath, Context context, DataHelper dataHelper = null)
        {
            if (!string.IsNullOrEmpty(filepath))
            {
                List<PatchedByteArray> patches = GetISOPatches(entryData, context, dataHelper);

                using (Stream file = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                {
                    PsxIso.PatchPsxIso(file, patches);
                }
            }
        }

        public static void SavePatchXML(EntryData entryData, string filepath, Context context, DataHelper dataHelper = null)
        {
            File.WriteAllText(filepath, CreatePatchXML(entryData, context, dataHelper), System.Text.Encoding.UTF8);
        }

        public static string CreatePatchXML(EntryData entryData, Context context, DataHelper dataHelper = null)
        {
            return PatcherLib.Utilities.Utilities.CreatePatchXML(GetISOPatches(entryData, context, dataHelper), Context.US_PSX, true, true, "EntryEdit Edits");
        }

        public static List<PatchedByteArray> GetISOPatches(EntryData entryData, Context context, DataHelper dataHelper = null)
        {
            dataHelper = dataHelper ?? new DataHelper(context);
            List<PatchedByteArray> patches = new List<PatchedByteArray>();

            SettingsData settings = Settings.GetSettings(context);

            Enum battleSector = settings.BattleConditionalsSector;
            int battleOffset = settings.BattleConditionalsOffset;
            byte[] battleBytes = dataHelper.ConditionalSetsToByteArray(CommandType.BattleConditional, entryData.BattleConditionals);
            patches.Add(new PatchedByteArray(battleSector, battleOffset, battleBytes));

            if ((settings.BattleConditionalsApplyLimitPatch) && (DataHelper.GetMaxBlocks(entryData.BattleConditionals) > 10))
            {
                patches.Add(new PatchedByteArray(settings.BattleConditionalsLimitPatchSector, settings.BattleConditionalsLimitPatchOffset, settings.BattleConditionalsLimitPatchBytes));
            }

            Enum worldSector = settings.WorldConditionalsSector;
            int worldOffset = settings.WorldConditionalsOffset;
            byte[] worldBytes = dataHelper.ConditionalSetsToByteArray(CommandType.WorldConditional, entryData.WorldConditionals);
            patches.Add(new PatchedByteArray(worldSector, worldOffset, worldBytes));

            if (settings.WorldConditionalsRepoint)
            {
                byte[] patchBytes = (((uint)(ISOHelper.GetRamOffsetUnsigned(worldSector, context, true) + worldOffset))).ToBytes();
                patches.Add(new PatchedByteArray(settings.WorldConditionalsPointerSector, settings.WorldConditionalsPointerOffset, patchBytes));
            }

            Enum eventSector = settings.EventsSector;
            int eventOffset = settings.EventsOffset;
            byte[] eventBytes = dataHelper.EventsToByteArray(entryData.Events);
            patches.Add(new PatchedByteArray(eventSector, eventOffset, eventBytes));

            return patches;
        }

        public static void PatchPsxSaveState(EntryData entryData, string filepath, DataHelper dataHelper)
        {
            SettingsData settings = Settings.PSX;

            Dictionary<uint, byte[]> ramPatches = new Dictionary<uint, byte[]>();
            bool isBattleLoaded = false;
            bool isWorldLoaded = false;
            bool saveBattleConditionals = false;
            bool saveWorldConditionals = false;
            bool saveEvent = false;
            byte[] worldConditionalsBytes = null;
            int battleConditionalsIndex = 0;
            int eventID = 0;

            int worldConditionalsRamLocation = 0;

            if (!string.IsNullOrEmpty(filepath))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filepath, FileMode.Open)))
                {
                    Stream stream = reader.BaseStream;

                    if (PsxIso.IsSectorInPsxSaveState(stream, PsxIso.Sectors.BATTLE_BIN))
                    {
                        isBattleLoaded = true;

                        saveBattleConditionals = (PsxIso.LoadFromPsxSaveState(reader, (uint)settings.BattleConditionalBlockOffsetsRAMLocation, 4).ToIntLE() != 0);
                        saveEvent = (PsxIso.LoadFromPsxSaveState(reader, (uint)settings.EventRAMLocation, 1).ToIntLE() != 0);

                        eventID = PsxIso.LoadFromPsxSaveState(reader, (uint)settings.EventIDRAMLocation, 2).ToIntLE();
                        if (!((eventID >= 0) && (eventID < entryData.Events.Count)))
                            saveEvent = false;

                        if (PsxIso.IsSectorInPsxSaveState(stream, (PsxIso.Sectors)settings.ScenariosSector))
                            battleConditionalsIndex = PsxIso.LoadFromPsxSaveState(reader, (uint)(settings.ScenariosRAMLocation + (eventID * 24) + 22), 2).ToIntLE();
                        else
                            saveBattleConditionals = false;
                    }
                    else if (PsxIso.IsSectorInPsxSaveState(stream, PsxIso.Sectors.WORLD_WLDCORE_BIN))
                    {
                        isWorldLoaded = true;

                        worldConditionalsRamLocation = settings.WorldConditionalsRepoint
                            ? PsxIso.LoadFromPsxSaveState(reader, (uint)settings.WorldConditionalsWorkingPointerRAMLocation, 3).ToIntLE()
                            : settings.WorldConditionalsCalcRAMLocation;

                        worldConditionalsBytes = dataHelper.ConditionalSetsToByteArray(CommandType.WorldConditional, entryData.WorldConditionals);
                        if (worldConditionalsBytes.Length > settings.WorldConditionalsSize)
                        {
                            saveWorldConditionals = false;
                        }  
                    }
                }

                using (BinaryReader reader = new BinaryReader(File.Open(filepath, FileMode.Open)))
                {
                    if (isBattleLoaded)
                    {
                        if (saveBattleConditionals)
                        {
                            int setIndex = battleConditionalsIndex;
                            if (setIndex >= 0)
                            {
                                uint blockRamOffset = (uint)settings.BattleConditionalBlockOffsetsRAMLocation;
                                uint commandRamOffset = (uint)settings.BattleConditionalsRAMLocation;

                                List<byte[]> byteArrays = dataHelper.ConditionalSetToActiveByteArrays(CommandType.BattleConditional, entryData.BattleConditionals[setIndex]);
                                ramPatches.Add(blockRamOffset, byteArrays[0]);
                                ramPatches.Add(commandRamOffset, byteArrays[1]);

                                if (settings.BattleConditionalsApplyLimitPatch)
                                {
                                    int numBlocks = entryData.BattleConditionals[setIndex].ConditionalBlocks.Count;
                                    if (numBlocks > 10)
                                    {
                                        ramPatches.Add((uint)settings.BattleConditionalsLimitPatchRAMLocation, settings.BattleConditionalsLimitPatchBytes);
                                    }
                                }
                            }
                        }

                        if (saveEvent)
                        {
                            int eventIndex = eventID;
                            if (eventIndex >= 0)
                            {
                                uint eventRamOffset = (uint)settings.EventRAMLocation;
                                byte[] eventBytes = dataHelper.EventToByteArray(entryData.Events[eventIndex], true);
                                ramPatches.Add(eventRamOffset, eventBytes);

                                uint eventRamOffsetKSeg0 = eventRamOffset | PsxIso.KSeg0Mask;
                                uint textOffset = eventBytes.SubLength(0, 4).ToUInt32();
                                if (textOffset != DataHelper.BlankTextOffsetValue)
                                {
                                    ramPatches.Add((uint)settings.TextOffsetRAMLocation, (eventRamOffsetKSeg0 + textOffset).ToBytes().ToArray());
                                }
                            }
                        }
                    }
                    else if (isWorldLoaded)
                    {
                        if (saveWorldConditionals)
                        {
                            uint ramOffset = (uint)worldConditionalsRamLocation;
                            ramPatches.Add(ramOffset, worldConditionalsBytes);
                            uint ramOffsetKSeg0 = ramOffset | PsxIso.KSeg0Mask;
                            if ((settings.WorldConditionalsRepoint) && (ramOffsetKSeg0 != PsxIso.LoadFromPsxSaveState(reader, (uint)settings.WorldConditionalsPointerRAMLocation, 4).ToUInt32()))
                            {
                                ramPatches.Add((uint)settings.WorldConditionalsPointerRAMLocation, ramOffsetKSeg0.ToBytes().ToArray());
                                ramPatches.Add((uint)settings.WorldConditionalsWorkingPointerRAMLocation, ramOffsetKSeg0.ToBytes().ToArray());
                            }
                        }
                    }

                    PsxIso.PatchPsxSaveState(reader, ramPatches);
                }
            }
        }
    }
}
