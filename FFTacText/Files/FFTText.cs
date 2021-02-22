/*
    Copyright 2007, Joe Davidson <joedavidson@gmail.com>

    This file is part of FFTPatcher.

    FFTPatcher is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FFTPatcher is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FFTPatcher.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using FFTPatcher.TextEditor.Files;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;
using PatcherLib.TextUtilities;

namespace FFTPatcher.TextEditor
{
    /// <summary>
    /// Represents a collection of FFT text files.
    /// </summary>
    public class FFTText
    {

        #region Fields (1)

        /// <summary>
        /// Gets the current version of FFTText files.
        /// </summary>
        public const int CurrentVersion = 3;

        #endregion Fields

        #region Properties (5)


        /// <summary>
        /// Gets the character map.
        /// </summary>
        public GenericCharMap CharMap { get; private set; }

        /// <summary>
        /// Gets the filetype.
        /// </summary>
        public Context Filetype { get; private set; }

        #endregion Properties

        #region Constructors (1)

        private FFTText()
        {
        }

        #endregion Constructors

        #region Methods (9)

        public delegate void PatchIso(Stream iso, IEnumerable<PatchedByteArray> patches);
        public class PatchIsoArgs
        {
            public string Filename { get; set; }
            public PatchIso Patcher { get; set; }
        }

        private struct DteResult
        {
            public enum Result
            {
                Success,
                Fail,
                Cancelled
            }

            public Result ResultCode;
            public ISerializableFile FailedFile;
            public static DteResult Empty { get { return new DteResult { ResultCode = Result.Fail }; } }
        }

        private DteResult DoDteForFiles(IList<ISerializableFile> dteFiles, BackgroundWorker worker, DoWorkEventArgs args,
            out IDictionary<ISerializableFile, Set<KeyValuePair<string, byte>>> preferredPairs,
            out Set<KeyValuePair<string, byte>> dtePairs)
        {
            var filePreferredPairs =
                new Dictionary<ISerializableFile, Set<KeyValuePair<string, byte>>>(dteFiles.Count);
            Set<KeyValuePair<string, byte>> currentPairs =
                new Set<KeyValuePair<string, byte>>((x, y) => x.Key.Equals(y.Key) && (x.Value == y.Value) ? 0 : -1);
            Stack<byte> dteBytes = DTE.GetAllowedDteBytes();
            var pairs = DTE.GetDteGroups(this.Filetype);
            foreach (var dte in dteFiles)
            {
                worker.ReportProgress(0,
                    new ProgressForm.FileProgress { File = dte, State = ProgressForm.TaskState.Starting, Task = ProgressForm.Task.CalculateDte });
                filePreferredPairs[dte] = dte.GetPreferredDTEPairs(pairs, currentPairs, dteBytes, worker);
                if (filePreferredPairs[dte] == null)
                {
                    dtePairs = null;
                    preferredPairs = null;
                    return new DteResult { ResultCode = DteResult.Result.Fail, FailedFile = dte };
                }
                currentPairs.AddRange(filePreferredPairs[dte]);
                worker.ReportProgress(0,
                    new ProgressForm.FileProgress { File = dte, State = ProgressForm.TaskState.Done, Task = ProgressForm.Task.CalculateDte });
                if (worker.CancellationPending)
                {
                    args.Cancel = true;
                    dtePairs = null;
                    preferredPairs = null;
                    return new DteResult { ResultCode = DteResult.Result.Cancelled };
                }
            }

            preferredPairs = new ReadOnlyDictionary<ISerializableFile, Set<KeyValuePair<string, byte>>>(filePreferredPairs);
            dtePairs = currentPairs;
            return new DteResult { ResultCode = DteResult.Result.Success };
        }

        private IList<PatchedByteArray> DoDteCrap(IList<ISerializableFile> dteFiles, BackgroundWorker worker, DoWorkEventArgs args)
        {
            List<PatchedByteArray> patches = new List<PatchedByteArray>();
            if (worker.CancellationPending)
            {
                args.Cancel = true;
                return null;
            }

            dteFiles.Sort((x, y) => (y.ToCDByteArray().Length - y.Layout.Size).CompareTo(x.ToCDByteArray().Length - x.Layout.Size));
            if (worker.CancellationPending)
            {
                args.Cancel = true;
                return null;
            }

            IDictionary<ISerializableFile, Set<KeyValuePair<string, byte>>> filePreferredPairs = null;
            Set<KeyValuePair<string, byte>> currentPairs = null;

            DteResult result = DteResult.Empty;
            if (dteFiles.Count > 0)
            {
                int tries = dteFiles.Count;
                //DteResult result = DoDteForFiles( dteFiles, worker, args, out filePreferredPairs, out currentPairs );
                do
                {
                    result = DoDteForFiles(dteFiles, worker, args, out filePreferredPairs, out currentPairs);
                    switch (result.ResultCode)
                    {
                        case DteResult.Result.Cancelled:
                            args.Cancel = true;
                            return null;
                        case DteResult.Result.Fail:
                            var failedFile = result.FailedFile;
                            if (dteFiles[0] == failedFile)
                            {
                                // Failed on the first file... this is hopeless
                                throw new FFTPatcher.TextEditor.DTE.DteException(failedFile);
                            }

                            // Bump the failed file to the top of the list
                            dteFiles.Remove(failedFile);
                            dteFiles.Insert(0, failedFile);
                            break;
                        case DteResult.Result.Success:
                            // do nothing
                            break;
                    }
                } while (result.ResultCode != DteResult.Result.Success && --tries >= 0);
            }

            switch (result.ResultCode)
            {
                case DteResult.Result.Fail:
                    throw new FFTPatcher.TextEditor.DTE.DteException(dteFiles[0]);
                case DteResult.Result.Cancelled:
                    args.Cancel = true;
                    return null;
            }

            foreach (var file in dteFiles)
            {
                worker.ReportProgress(0,
                    new ProgressForm.FileProgress { File = file, State = ProgressForm.TaskState.Starting, Task = ProgressForm.Task.GeneratePatch });
                var currentFileEncoding = PatcherLib.Utilities.Utilities.DictionaryFromKVPs(filePreferredPairs[file]);
                patches.AddRange(file.GetDtePatches(currentFileEncoding));
                worker.ReportProgress(0,
                    new ProgressForm.FileProgress { File = file, State = ProgressForm.TaskState.Done, Task = ProgressForm.Task.GeneratePatch });
                if (worker.CancellationPending)
                {
                    args.Cancel = true;
                    return null;
                }
            }

            patches.AddRange(DTE.GenerateDtePatches(this.Filetype, currentPairs));
            return patches.AsReadOnly();
        }

        public void BuildAndApplyPatches(object sender, DoWorkEventArgs args)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            PatchIsoArgs patchArgs = args.Argument as PatchIsoArgs;
            if (patchArgs == null)
            {
                throw new Exception("Incorrect args passed to BuildAndApplyPatches");
            }
            if (patchArgs.Patcher == null)
            {
                throw new ArgumentNullException("Patcher", "Patcher cannot be null");
            }

            using (Stream stream = File.Open(patchArgs.Filename, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            {
                if (stream == null)
                {
                    throw new Exception("Could not open ISO file");
                }

                IList<ISerializableFile> files = new List<ISerializableFile>(Files.Count);
                Files.FindAll(f => f is ISerializableFile).ForEach(f => files.Add((ISerializableFile)f));

                List<ISerializableFile> dteFiles = new List<ISerializableFile>();
                List<ISerializableFile> nonDteFiles = new List<ISerializableFile>();
                List<PatchedByteArray> patches = new List<PatchedByteArray>();

                foreach (ISerializableFile file in files)
                {
                    worker.ReportProgress(0,
                        new ProgressForm.FileProgress { File = file, State = ProgressForm.TaskState.Starting, Task = ProgressForm.Task.IsDteNeeded });
                    if (file.IsDteNeeded())
                    {
                        dteFiles.Add(file);
                    }
                    else
                    {
                        nonDteFiles.Add(file);
                    }
                    worker.ReportProgress(0,
                        new ProgressForm.FileProgress { File = file, State = ProgressForm.TaskState.Done, Task = ProgressForm.Task.IsDteNeeded });
                    if (worker.CancellationPending)
                    {
                        args.Cancel = true;
                        return;
                    }
                }
                if (dteFiles.Count > 0)
                {
                    var dtePatches = DoDteCrap(dteFiles, worker, args);
                    if (dtePatches == null)
                    {
                        args.Cancel = true;
                        return;
                    }
                    patches.AddRange(dtePatches);
                }

                foreach (var file in nonDteFiles)
                {
                    worker.ReportProgress(0,
                        new ProgressForm.FileProgress { File = file, State = ProgressForm.TaskState.Starting, Task = ProgressForm.Task.GeneratePatch });
                    patches.AddRange(file.GetNonDtePatches());
                    worker.ReportProgress(0,
                        new ProgressForm.FileProgress { File = file, State = ProgressForm.TaskState.Done, Task = ProgressForm.Task.GeneratePatch });
                    if (worker.CancellationPending)
                    {
                        args.Cancel = true;
                        return;
                    }
                } 

                if (worker.CancellationPending)
                {
                    args.Cancel = true;
                    return;
                }

                worker.ReportProgress(0,
                    new ProgressForm.FileProgress { File = null, State = ProgressForm.TaskState.Starting, Task = ProgressForm.Task.ApplyingPatches });
                patchArgs.Patcher(stream, patches);
                worker.ReportProgress(0,
                    new ProgressForm.FileProgress { File = null, State = ProgressForm.TaskState.Done, Task = ProgressForm.Task.ApplyingPatches });

            }
        }

        public void PatchISOSimple(string filename)
        {
            using (Stream stream = File.Open(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            {
                if (stream == null)
                {
                    throw new Exception("Could not open ISO file");
                }

                IList<ISerializableFile> files = new List<ISerializableFile>(Files.Count);
                Files.FindAll(f => f is ISerializableFile).ForEach(f => files.Add((ISerializableFile)f));

                List<ISerializableFile> dteFiles = new List<ISerializableFile>();
                List<ISerializableFile> nonDteFiles = new List<ISerializableFile>();
                List<PatchedByteArray> patches = new List<PatchedByteArray>();

                foreach (ISerializableFile file in files)
                {
                    if (file.IsDteNeeded())
                    {
                        dteFiles.Add(file);
                    }
                    else
                    {
                        nonDteFiles.Add(file);
                    }
                }
                if (dteFiles.Count > 0)
                {
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.WorkerReportsProgress = true;

                    var dtePatches = DoDteCrap(dteFiles, worker, new DoWorkEventArgs(""));
                    if (dtePatches == null)
                    {
                        return;
                    }
                    patches.AddRange(dtePatches);
                }

                foreach (var file in nonDteFiles)
                {
                    patches.AddRange(file.GetNonDtePatches());
                }

                if (Filetype == Context.US_PSP)
                {
                    PatcherLib.Iso.PspIso.PatchISO(stream, patches);
                }
                else if (Filetype == Context.US_PSX)
                {
                    PatcherLib.Iso.PsxIso.PatchPsxIso(stream, patches);
                }
            }
        }

        private void WriteChangesToFile(IEnumerable<PatchedByteArray> patches, string filepath)
        {
            PatcherLib.Utilities.Utilities.WriteChangesToFile(patches, filepath);
        }

        public void GenerateResourcesZip(string filepath)
        {
            Dictionary<string, AbstractFile> files = new Dictionary<string, AbstractFile>(Files.Count);
            foreach (IFile file in Files)
            {
                if (file is AbstractFile)
                    files.Add(file.DisplayName, (AbstractFile)file);
            }

            Dictionary<string, IList<byte>> zipFileContents = new Dictionary<string, IList<byte>>();
            IDictionary<string, IList<byte>> defaultZipFileContents = PatcherLib.ResourcesClass.DefaultZipFileContents;
            foreach (KeyValuePair<string, IList<byte>> entry in defaultZipFileContents)
            {
                zipFileContents.Add(entry.Key, GetResourcesBytes(entry.Key, entry.Value, files));
            }

            PatcherLib.ResourcesClass.GenerateResourcesZip(filepath, zipFileContents);
        }

        private IList<byte> GetResourcesBytes(string xmlFilename, IList<byte> defaultBytes, IDictionary<string, AbstractFile> files)
        {
            IList<byte> result = null;
            string[] section = null;

            if (Filetype == Context.US_PSP)
            {
                switch (xmlFilename)
                {
                    case PatcherLib.ResourcesClass.Paths.PSP.AbilitiesNamesXML:
                        section = PatcherLib.ResourcesClass.EnforceUniqueStrings(files["BOOT.BIN 3"].Sections[0]);
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(section, "Abilities", "Ability", "name", "value", false);
                        break;
                    case PatcherLib.ResourcesClass.Paths.PSP.ChroniclesXML:
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(new string[2] {
                            PatcherLib.ResourcesClass.GenerateXMLSectionString(files["WORLD.LZW"].Sections[27], "Wonders", "Entry", null, "value", false),
                            PatcherLib.ResourcesClass.GenerateXMLSectionString(files["WORLD.LZW"].Sections[28], "Artefacts", "Entry", null, "value", false)
                        }, "Chron");
                        break;
                    case PatcherLib.ResourcesClass.Paths.PSP.ItemsXML:
                        section = PatcherLib.ResourcesClass.EnforceUniqueStrings(files["BOOT.BIN[28E5EC]"].Sections[7]);
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(section, "Items", "Item", "name", "offset", false);
                        break;
                    case PatcherLib.ResourcesClass.Paths.PSP.JobsXML:
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(files["BOOT.BIN 2"].Sections[0], "Jobs", "Job", "name", "offset", true);
                        break;
                    case PatcherLib.ResourcesClass.Paths.PSP.MapNamesXML:
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(files["WORLD.LZW"].Sections[21], "MapNames", "Map", null, "value", false);
                        break;
                    case PatcherLib.ResourcesClass.Paths.PSP.PropositionsXML:
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(files["WORLD.LZW"].Sections[26], "Errands", "entry", "name", "value", false);
                        break;
                    case PatcherLib.ResourcesClass.Paths.PSP.SkillSetsXML:
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(files["BOOT.BIN 1"].Sections[0], "SkillSets", "SkillSet", "name", "byte", true);
                        break;
                    case PatcherLib.ResourcesClass.Paths.PSP.StatusNamesXML:
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(files["BOOT.BIN[28E5EC]"].Sections[17], "Statuses", "Status", "name", "offset", false);
                        break;
                    case PatcherLib.ResourcesClass.Paths.PSP.UnitNamesXML:
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(files["BOOT.BIN[29E334]"].Sections[1], "Names", "n", null, "id", false);
                        break;
                    default:
                        result = defaultBytes;
                        break;
                }
            }
            else if (Filetype == Context.US_PSX)
            {
                switch (xmlFilename)
                {
                    case PatcherLib.ResourcesClass.Paths.PSX.AbilitiesNamesXML:
                        section = PatcherLib.ResourcesClass.EnforceUniqueStrings(files["WORLD.BIN 03"].Sections[0]);
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(section, "Abilities", "Ability", "name", "value", false);
                        break;
                    case PatcherLib.ResourcesClass.Paths.PSX.BraveStoryXML:
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(new string[2] {
                            PatcherLib.ResourcesClass.GenerateXMLSectionString(files["WORLD.LZW"].Sections[27], "UnexploredLands", "entry", "Name", "value", false),
                            PatcherLib.ResourcesClass.GenerateXMLSectionString(files["WORLD.LZW"].Sections[28], "Treasures", "entry", "Name", "value", false)
                        }, "BS");
                        break;
                    case PatcherLib.ResourcesClass.Paths.PSX.ItemsXML:
                        section = PatcherLib.ResourcesClass.EnforceUniqueStrings(files["WORLD.LZW"].Sections[7]);
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(section, "Items", "Item", "name", "offset", false);
                        break;
                    case PatcherLib.ResourcesClass.Paths.PSX.JobsXML:
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(files["WORLD.BIN 02"].Sections[0], "Jobs", "Job", "name", "offset", true);
                        break;
                    case PatcherLib.ResourcesClass.Paths.PSX.MapNamesXML:
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(files["WORLD.LZW"].Sections[21], "MapNames", "Map", null, "value", false);
                        break;
                    case PatcherLib.ResourcesClass.Paths.PSX.PropositionsXML:
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(files["WORLD.LZW"].Sections[26], "Propositions", "entry", "name", "value", false);
                        break;
                    case PatcherLib.ResourcesClass.Paths.PSX.SkillSetsXML:
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(files["BATTLE.BIN"].Sections[22], "SkillSets", "SkillSet", "name", "byte", true);
                        break;
                    case PatcherLib.ResourcesClass.Paths.PSX.StatusNamesXML:
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(files["BATTLE.BIN"].Sections[17], "Statuses", "Status", "name", "offset", false);
                        break;
                    case PatcherLib.ResourcesClass.Paths.PSX.UnitNamesXML:
                        result = PatcherLib.ResourcesClass.GenerateXMLBytes(files["ATTACK.OUT"].Sections[1], "Names", "n", null, "id", false);
                        break;
                    default:
                        result = defaultBytes;
                        break;
                }
            }

            return result;
        }

        public void RegulateNewlines()
        {
            string newNewline = String.Format("{{Newline}}{0}", Environment.NewLine);

            foreach (IFile file in Files)
            {
                if (file is QuickEdit)
                {
                    QuickEdit quickEdit = (QuickEdit)file;
                    int numSections = quickEdit.NumberOfSections;
                    for (int sectionIndex = 0; sectionIndex < numSections; sectionIndex++)
                    {
                        int numEntries = quickEdit.SectionLengths[sectionIndex];
                        for (int entryIndex = 0; entryIndex < numEntries; entryIndex++)
                        {
                            if (!string.IsNullOrEmpty(quickEdit[sectionIndex, entryIndex]))
                            {
                                quickEdit.UpdateEntry(sectionIndex, entryIndex, quickEdit[sectionIndex, entryIndex].Replace("\r", "").Replace("\n", "").Replace("{Newline}", newNewline));
                            }
                        }
                    }
                }
                if (file is AbstractFile)
                {
                    AbstractFile aFile = (AbstractFile)file;
                    foreach (IList<string> section in aFile.Sections)
                    {
                        for (int index = 0; index < section.Count; index++)
                        {
                            if (!string.IsNullOrEmpty(section[index]))
                            {
                                section[index] = section[index].Replace("\r", "").Replace("\n", "").Replace("{Newline}", newNewline);
                            }
                        }
                    }
                }
            }
        }

        public IList<IFile> Files { get; private set; }

        internal FFTText(Context context, IDictionary<Guid, ISerializableFile> files, QuickEdit quickEdit)
        {
            Filetype = context;
            List<IFile> filesList = new List<IFile>(files.Count + 1);
            files.ForEach(kvp => filesList.Add(kvp.Value));
            filesList.Sort((a, b) => a.DisplayName.CompareTo(b.DisplayName));
            if (quickEdit != null)
            {
                filesList.Insert(0, quickEdit);
            }
            Files = filesList.AsReadOnly();
            CharMap = filesList[0].CharMap;
        }

        public static FFTText ReadPSPIso(string filename, BackgroundWorker worker)
        {
            using (FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return ReadPSPIso(stream, worker);
            }
        }

        public static FFTText ReadPSPIso(FileStream stream, BackgroundWorker worker)
        {
            return FFTTextFactory.GetPspText(stream, worker);
        }

        public static FFTText ReadPSXIso(string filename, BackgroundWorker worker)
        {
            using (FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return ReadPSXIso(stream, worker);
            }
        }

        public static FFTText ReadPSXIso(FileStream stream, BackgroundWorker worker)
        {
            return FFTTextFactory.GetPsxText(stream, worker);
        }

        #endregion Methods
    }
}
