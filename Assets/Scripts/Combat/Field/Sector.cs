using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmniGlyph.Actors;
using OmniGlyph.Configs;
using OmniGlyph.Internals;
using OmniGlyph.Internals.Debugging;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace OmniGlyph.Combat.Field {
    public class Sector : OmniMono {
        private bool _isRoot = false;
        [SerializeField]
        private List<Actor> _actors;
        [SerializeField]
        private CombatRanges _combatRange;
        [SerializeField]
        private Side _side;
        [SerializeField]
        private float _stripeSize;
        public List<Actor> Actors { get { return _actors; } private set { _actors = value; } }

        public static event Action<Sector, Sector, Actor> ActorMoved;
        public CombatRanges CombatRange { get { return _combatRange; } set { _combatRange = value; } }
        public Side Side { get { return _side; } private set { _side = value; } }

        private SectorStrip[] _sectorStrips;
        private string GetSectorName() {
            return $"Sector {CombatRange} {Side}";
        }
        private void SectorInit(Func<Vector3, Sector, Sector> sectorSpawner, Side side, CombatRanges combatRange, float stripeSize) {
            SectorInit(side, combatRange, stripeSize);
            tag = "CombatSector";
            SpawnChildSector(sectorSpawner, side, stripeSize);
        }
        private void SectorInit(Side side, CombatRanges combatrange, float stripeSize) {
            CombatRange = combatrange;
            Side = side;
            Actors = new List<Actor>();
            name = GetSectorName();

            GenerateStrips(stripeSize);
            Debugger.Log($"Sector {name} initialized with size {transform.lossyScale}");
            Debugger.Watch(new DebugObjectProperties(transform.lossyScale * 0.9f, Color.red, PrimitiveType.Cube));
        }
        public void SectorInit(Func<Vector3, Sector, Sector> sectorSpawner, float stripeSize) {
            if (transform.parent != null && transform.parent.GetComponent<Sector>() != null) {
                Debugger.ThrowCriticalError("SectorInit should only be called on the root sector");
            }
            _isRoot = true;
            SectorInit(Side.Middle, CombatRanges.Close, stripeSize);
            tag = "RootCombatSector";
            for (int i = -1; i <= 1; i += 2) {
                if (!SpawnChildSector(sectorSpawner, (Side)i, stripeSize).HasValue) {
                    Debugger.ThrowCriticalError("Failed to spawn child sector");
                }
            }
        }
        private void GenerateStrips(float stripeSize) {
            int stripCount = Mathf.FloorToInt(transform.lossyScale.z / stripeSize);
            float remainingSizeZ = transform.lossyScale.z - (stripCount * stripeSize);
            float offset = remainingSizeZ / 2f + stripeSize / 2f;
            _sectorStrips = new SectorStrip[stripCount];

            Debugger.Log($"Size = {stripeSize}\nCount = {stripCount}\nremainingSizeZ = {remainingSizeZ}\noffset={offset}");
            for (int i = 0; i < _sectorStrips.Length; i++) {
                float zPos = transform.position.z - (transform.lossyScale.z / 2f) + offset + (stripeSize * i);
                Vector3 stripCenter = new Vector3(transform.position.x, transform.position.y, zPos);

                GameObject strip = new GameObject($"{name} Strip {i}");
                strip.transform.position = stripCenter;
                strip.transform.localScale = new Vector3(transform.lossyScale.x, transform.lossyScale.y, stripeSize);
                strip.transform.SetParent(transform, true);
                strip.name = $"{name} Strip {i}";
                _sectorStrips[i] = strip.AddComponent<SectorStrip>();
            }
        }
        private Maybe<Sector> SpawnChildSector(Func<Vector3, Sector, Sector> sectorSpawner, Side side, float stripeSize) {
            short lastEnum = Enum.GetValues(typeof(CombatRanges)).Cast<short>().Max();
            if (lastEnum == (short)CombatRange) {
                return Maybe<Sector>.None();
            }
            Sector s = sectorSpawner(transform.position + Vector3.Scale(transform.lossyScale, Vector3.right) * (side == Side.Left ? -1 : 1), this);
            s.SectorInit(sectorSpawner, side, Enum.Parse<CombatRanges>(((short)CombatRange << 1).ToString()), stripeSize);
            return Maybe<Sector>.Some(s);
        }
        public void DestroySector() {
            Debugger.Log($"Destroying sector {name}, is root: {_isRoot}");

            foreach (Transform child in transform) {
                if (child == this) {
                    continue;
                }
                child.GetComponent<Sector>()?.DestroySector();
            }
            Debugger.Log($"Sector destroyed with position: {transform.position}");
            Destroy(gameObject);
        }
        public Sector GetRoot() {
            if (_isRoot) {
                return this;
            }
            return transform.parent.GetComponent<Sector>().GetRoot();
        }
        public Maybe<SectorStrip> AddActor(Side sectorSide, Actor actor) {
            foreach (int i in ActorStripeIndexGenerator()) {
                Debugger.Log($"Checking strip {i}/{_sectorStrips.Length - 1} for actor {actor.name}");
                if (_sectorStrips[i].HasActor(actor)) {
                    Debugger.ThrowCriticalError($"Actor {actor.name} already exists in sector {name}");
                } else if (_sectorStrips[i].HasActor(sectorSide)) {
                    continue;
                }
                Debugger.Log($"Adding actor {actor.name} to strip {i}/{_sectorStrips.Length - 1} at {_sectorStrips[i].Center}");
                _sectorStrips[i].SetActor(sectorSide, actor);
                return Maybe<SectorStrip>.Some(_sectorStrips[i]);
            }
            return Maybe<SectorStrip>.None();
        }
        public void RemoveActor(Actor actor) {
            foreach (int i in ActorStripeIndexGenerator()) {
                if (_sectorStrips[i].HasActor(actor)) {
                    _sectorStrips[i].RemoveActor(actor);
                    return;
                }

                Debugger.ThrowCriticalError($"Actor {actor.name} does not exist in sector {name}");
            }
        }
        private IEnumerable<int> ActorStripeIndexGenerator() {
            int middleIndex = Mathf.FloorToInt((_sectorStrips.Length - 1) / 2f);

            for (int i = 1; i < _sectorStrips.Length; i++) {
                int indexMod = (i % 2 == 0 ? 1 : -1) * Mathf.FloorToInt(i / 2f);
                yield return middleIndex + indexMod;
            }
        }

        public Sector GetSector(Side side, CombatRanges range) {
            Debugger.Log($"Checking sector {name} for side {side} and range {range}; This sector side: {Side} range: {CombatRange}");
            if (Side == side && CombatRange == range) {
                return this;
            }
            foreach (Transform child in transform) {
                Sector s = child.GetComponent<Sector>()?.GetSector(side, range);
                if (s != null) {
                    return s;
                }
            }
            return null;
        }
    }
}
