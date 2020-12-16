using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PatcherLib.Datatypes;
using PatcherLib.Iso;
using PatcherLib.Utilities;

namespace EntryEdit
{
    public class PatchHelper
    {
        public static EntryData GetEntryDataFromPatchFile(string filepath, DataHelper dataHelper = null)
        {
            dataHelper = dataHelper ?? new DataHelper();

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

        public static void PatchISO(EntryData entryData, string filepath, DataHelper dataHelper = null)
        {
            if (!string.IsNullOrEmpty(filepath))
            {
                dataHelper = dataHelper ?? new DataHelper();
                List<PatchedByteArray> patches = new List<PatchedByteArray>();

                PsxIso.Sectors battleSector = Settings.BattleConditionalsSector;
                int battleOffset = Settings.BattleConditionalsOffset;
                byte[] battleBytes = dataHelper.ConditionalSetsToByteArray(CommandType.BattleConditional, entryData.BattleConditionals);
                patches.Add(new PatchedByteArray(battleSector, battleOffset, battleBytes));

                if ((Settings.BattleConditionalsApplyLimitPatch) && (DataHelper.GetMaxBlocks(entryData.BattleConditionals) > 10))
                {
                    patches.Add(new PatchedByteArray(Settings.BattleConditionalsLimitPatchSector, Settings.BattleConditionalsLimitPatchOffset, Settings.BattleConditionalsLimitPatchBytes));
                }

                PsxIso.Sectors worldSector = Settings.WorldConditionalsSector;
                int worldOffset = Settings.WorldConditionalsOffset;
                byte[] worldBytes = dataHelper.ConditionalSetsToByteArray(CommandType.WorldConditional, entryData.WorldConditionals);
                patches.Add(new PatchedByteArray(worldSector, worldOffset, worldBytes));

                if (Settings.WorldConditionalsRepoint)
                {
                    byte[] patchBytes = (((uint)(PsxIso.GetRamOffset(worldSector) + worldOffset)) | PsxIso.KSeg0Mask).ToBytes();
                    patches.Add(new PatchedByteArray(Settings.WorldConditionalsPointerSector, Settings.WorldConditionalsPointerOffset, patchBytes));
                }

                PsxIso.Sectors eventSector = Settings.EventsSector;
                int eventOffset = Settings.EventsOffset;
                byte[] eventBytes = dataHelper.EventsToByteArray(entryData.Events);
                patches.Add(new PatchedByteArray(eventSector, eventOffset, eventBytes));
                
                using (Stream file = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite))
                {
                    PsxIso.PatchPsxIso(file, patches);
                }
            }
        }

        public static void PatchPsxSaveState(EntryData entryData, string filepath, DataHelper dataHelper = null)
        {
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
                dataHelper = dataHelper ?? new DataHelper();

                using (BinaryReader reader = new BinaryReader(File.Open(filepath, FileMode.Open)))
                {
                    Stream stream = reader.BaseStream;

                    if (PsxIso.IsSectorInPsxSaveState(stream, PsxIso.Sectors.BATTLE_BIN))
                    {
                        isBattleLoaded = true;

                        saveBattleConditionals = (PsxIso.LoadFromPsxSaveState(reader, (uint)Settings.BattleConditionalBlockOffsetsRAMLocation, 4).ToIntLE() != 0);
                        saveEvent = (PsxIso.LoadFromPsxSaveState(reader, (uint)Settings.EventRAMLocation, 1).ToIntLE() != 0);

                        eventID = PsxIso.LoadFromPsxSaveState(reader, (uint)Settings.EventIDRAMLocation, 2).ToIntLE();
                        if (!((eventID >= 0) && (eventID < entryData.Events.Count)))
                            saveEvent = false;

                        if (PsxIso.IsSectorInPsxSaveState(stream, Settings.ScenariosSector))
                            battleConditionalsIndex = PsxIso.LoadFromPsxSaveState(reader, (uint)(Settings.ScenariosRAMLocation + (eventID * 24) + 22), 2).ToIntLE();
                        else
                            saveBattleConditionals = false;
                    }
                    else if (PsxIso.IsSectorInPsxSaveState(stream, PsxIso.Sectors.WORLD_WLDCORE_BIN))
                    {
                        isWorldLoaded = true;

                        worldConditionalsRamLocation = Settings.WorldConditionalsRepoint
                            ? PsxIso.LoadFromPsxSaveState(reader, (uint)Settings.WorldConditionalsWorkingPointerRAMLocation, 3).ToIntLE()
                            : Settings.WorldConditionalsCalcRAMLocation;

                        worldConditionalsBytes = dataHelper.ConditionalSetsToByteArray(CommandType.WorldConditional, entryData.WorldConditionals);
                        if (worldConditionalsBytes.Length > Settings.WorldConditionalsSize)
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
                                uint blockRamOffset = (uint)Settings.BattleConditionalBlockOffsetsRAMLocation;
                                uint commandRamOffset = (uint)Settings.BattleConditionalsRAMLocation;

                                List<byte[]> byteArrays = dataHelper.ConditionalSetToActiveByteArrays(CommandType.BattleConditional, entryData.BattleConditionals[setIndex]);
                                ramPatches.Add(blockRamOffset, byteArrays[0]);
                                ramPatches.Add(commandRamOffset, byteArrays[1]);

                                if (Settings.BattleConditionalsApplyLimitPatch)
                                {
                                    int numBlocks = entryData.BattleConditionals[setIndex].ConditionalBlocks.Count;
                                    if (numBlocks > 10)
                                    {
                                        ramPatches.Add((uint)Settings.BattleConditionalsLimitPatchRAMLocation, Settings.BattleConditionalsLimitPatchBytes);
                                    }
                                }
                            }
                        }

                        if (saveEvent)
                        {
                            int eventIndex = eventID;
                            if (eventIndex >= 0)
                            {
                                uint eventRamOffset = (uint)Settings.EventRAMLocation;
                                byte[] eventBytes = dataHelper.EventToByteArray(entryData.Events[eventIndex], true);
                                ramPatches.Add(eventRamOffset, eventBytes);

                                uint eventRamOffsetKSeg0 = eventRamOffset | PsxIso.KSeg0Mask;
                                uint textOffset = eventBytes.SubLength(0, 4).ToUInt32();
                                if (textOffset != DataHelper.BlankTextOffsetValue)
                                {
                                    ramPatches.Add((uint)Settings.TextOffsetRAMLocation, (eventRamOffsetKSeg0 + textOffset).ToBytes().ToArray());
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
                            if ((Settings.WorldConditionalsRepoint) && (ramOffsetKSeg0 != PsxIso.LoadFromPsxSaveState(reader, (uint)Settings.WorldConditionalsPointerRAMLocation, 4).ToUInt32()))
                            {
                                ramPatches.Add((uint)Settings.WorldConditionalsPointerRAMLocation, ramOffsetKSeg0.ToBytes().ToArray());
                                ramPatches.Add((uint)Settings.WorldConditionalsWorkingPointerRAMLocation, ramOffsetKSeg0.ToBytes().ToArray());
                            }
                        }
                    }

                    PsxIso.PatchPsxSaveState(reader, ramPatches);
                }
            }
        }
    }
}
