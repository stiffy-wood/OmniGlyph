using System;
using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Combat;
using OmniGlyph.Internals;
using UnityEngine;
using UnityEngine.XR;

namespace OmniGlyph.Combat.Field {
    public class SectorData {
        private class CacheKeys {
            public static string SIDE = "side";
            public static string RANGE = "range";
        }
        private Dictionary<string, object> _cache = new Dictionary<string, object>();

        private SectorPosition _sectorPosition;
        public string SectorName { get { return $"Sector {GetRange()} {GetSide()}"; } }
        public SectorPosition SectorPosition { get { return _sectorPosition; } }

        private Side GetSideFromCache() {
            bool exists = _cache.TryGetValue(CacheKeys.SIDE, out object side);
            if (!exists) {
                side = GetSide(_sectorPosition);
                _cache[CacheKeys.SIDE] = side;
            }
            return (Side)side;
        }
        private CombatRanges GetRangeFromCache() {
            bool exists = _cache.TryGetValue(CacheKeys.RANGE, out object range);
            if (!exists) {
                range = GetRange(_sectorPosition);
                _cache[CacheKeys.RANGE] = range;
            }
            return (CombatRanges)range;
        }
        public bool IsRight() {
            return GetSideFromCache() == Side.Right;
        }
        public bool IsLeft() {
            return GetSideFromCache() == Side.Left;
        }
        public bool IsCenter() {
            return GetSideFromCache() == Side.Middle;
        }
        public bool IsClose() {
            return GetRangeFromCache() == CombatRanges.Close;
        }
        public bool IsMid() {
            return GetRangeFromCache() == CombatRanges.Mid;
        }
        public bool IsFar() {
            return GetRangeFromCache() == CombatRanges.Far;
        }
        public void SetPos(Side side, CombatRanges range) {
            SetPos(NewSectorPos(side, range));
        }
        public void SetPos(SectorPosition sectorPos) {
            _cache = new Dictionary<string, object>();
            _sectorPosition = sectorPos;
        }
        public CombatRanges GetRange() {
            return GetRangeFromCache();
        }
        public static CombatRanges GetRange(SectorPosition sectorPosition) {
            return (CombatRanges)Enum.ToObject(typeof(CombatRanges), (byte)sectorPosition >> 2);
        }
        public Side GetSide() {
            return GetSideFromCache();
        }
        public static Side GetSide(SectorPosition sectorPosition) {
            return (Side)Enum.ToObject(typeof(Side), (byte)sectorPosition & (byte)SectorPosition.Center);
        }
        public (Side side, CombatRanges range) SplitPosition() {
            return SplitPosition(_sectorPosition);
        }
        public static (Side side, CombatRanges range) SplitPosition(SectorPosition sectorPosition) {
            return (
                GetSide(sectorPosition),
                GetRange(sectorPosition)
                );
        }
        public static SectorPosition NewSectorPos(Side side, CombatRanges range) {
            return (SectorPosition)Enum.ToObject(typeof(SectorPosition), (byte)side | ((byte)range << 2));
        }
        public CombatRanges GetDistance(SectorData other) {
            return GetDistance(other.SectorPosition);
        }
        public CombatRanges GetDistance(SectorPosition other) {
            return GetDistance(_sectorPosition, other);
        }
        public static CombatRanges GetDistance(SectorData data1, SectorData data2) {
            return GetDistance(data1.SectorPosition, data2.SectorPosition);
        }
        public static CombatRanges GetDistance(SectorPosition pos1, SectorPosition pos2) {
            if (pos1 == pos2) {
                return CombatRanges.Close;
            }
            (Side side, CombatRanges range) split1 = SplitPosition(pos1);
            (Side side, CombatRanges range) split2 = SplitPosition(pos2);

            bool areSameSide = ((byte)split1.side & (byte)split2.side) > 0;
            int distance = Math.Abs((byte)split1.range - (byte)split2.range);

            if (distance <= 2 && areSameSide) {
                return CombatRanges.Mid;
            }
            return CombatRanges.Far;
        }
    }
}
