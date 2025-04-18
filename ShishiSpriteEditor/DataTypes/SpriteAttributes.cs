﻿using System;
using System.Collections.Generic;
using System.Text;
using PatcherLib.Iso;
using System.IO;
using static PatcherLib.Iso.PspIso;
using System.Runtime.InteropServices.ComTypes;

namespace FFTPatcher.SpriteEditor
{
    public enum SpriteType
    {
        TYPE1 = 0,
        TYPE2 = 1,
        CYOKO = 2,
        MON = 3,
        OTHER = 4,
        RUKA = 5,
        ARUTE = 6,
        KANZEN = 7,
        WEP1 = 8,
        WEP2 = 9,
        EFF1 = 10,
        EFF2 = 11
    }

    internal class SpriteAttributes
    {
        public SpriteType SHP { get; private set;}
        public SpriteType SEQ { get; private set; }
        public bool Flying { get; private set; }
        public byte Height { get; private set; }

        internal void SetSHP(Stream iso, SpriteType shp, bool hasDataChange)
        {
            SHP = shp;
            UpdateIso(iso, hasDataChange);
        }

        internal void SetSEQ(Stream iso, SpriteType seq, bool hasDataChange)
        {
            SEQ = seq;
            UpdateIso(iso, hasDataChange);
        }

        internal void SetFlying(Stream iso, bool flying, bool hasDataChange)
        {
            Flying = flying;
            UpdateIso(iso, hasDataChange);
        }

        internal void SetHeight(Stream iso, byte height, bool hasDataChange)
        {
            Height = height;
            UpdateIso(iso, hasDataChange);
        }

        private void UpdateIso(Stream iso, bool hasDataChange)
        {
            if (psxPos != null)
            {
                PsxIso.PatchPsxIso(iso, psxPos.GetPatchedByteArray(ToByteArray()));
            }
            else if (pspPos != null)
            {
                if (!hasDataChange)
                {
                    PspIsoInfo info = PspIsoInfo.GetPspIsoInfo(iso);
                    PspIso.DecryptISO(iso, info);
                }

                PspIso.KnownPosition ebootPos = new PspIso.KnownPosition(PspIso.Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, pspPos.StartLocation, pspPos.Length);
                byte[] bytes = ToByteArray();

                PspIso.ApplyPatch(iso, pspInfo, pspPos.GetPatchedByteArray(bytes));
                PspIso.ApplyPatch(iso, pspInfo, ebootPos.GetPatchedByteArray(bytes));
            }
        }

        internal void SetAttributes(IList<byte> bytes)
        {
            System.Diagnostics.Debug.Assert(bytes.Count == 4);
            SHP = (SpriteType)bytes[0];
            SEQ = (SpriteType)bytes[1];
            Flying = bytes[2] != 0;
            Height = bytes[3];
        }

        private SpriteAttributes(PsxIso.KnownPosition pos, IList<byte> bytes)
        {
            SetAttributes(bytes);
            psxPos = pos;
        }

        private SpriteAttributes(PspIso.KnownPosition pos, PspIso.PspIsoInfo info, IList<byte> bytes)
        {
            SetAttributes(bytes);
            pspPos = pos;
            pspInfo = info;
        }

        private PsxIso.KnownPosition psxPos;
        private PspIso.KnownPosition pspPos;
        private PspIso.PspIsoInfo pspInfo;

        public static SpriteAttributes BuildPsx(PsxIso.KnownPosition pos, IList<byte> bytes)
        {
            return new SpriteAttributes(pos, bytes);
        }

        public static SpriteAttributes BuildPsp(PspIso.KnownPosition pos, PspIso.PspIsoInfo info, IList<byte> bytes)
        {
            return new SpriteAttributes(pos, info, bytes);
        }

        internal byte[] ToByteArray()
        {
            byte[] result = new byte[4];
            result[0] = (byte)SHP;
            result[1] = (byte)SEQ;
            result[2] = Flying ? (byte)1 : (byte)0;
            result[3] = Height;
            return result;
        }

    }
}
