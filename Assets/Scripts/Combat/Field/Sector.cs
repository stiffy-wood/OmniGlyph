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
    public class Sector : OmniMonoInstance {
        private bool _isRoot = false;
        [SerializeField]
        private List<Actor> _actors;
        [SerializeField]
        private float _stripeSize;
        [SerializeField]
        private SectorData _sectorData;

        public List<Actor> Actors { get { return _actors; } private set { _actors = value; } }

        public static event Action<Sector, Sector, Actor> ActorMoved;
        //public CombatRanges CombatRange { get { return _combatRange; } set { _combatRange = value; } }
        //public Side Side { get { return _side; } private set { _side = value; } }
        public SectorData SectorData { get { return _sectorData; } }
        private SectorStrip[] _sectorStrips;

        // Non-root sector init
        private void SectorInit(Func<Vector3, Sector, Sector> sectorSpawner, Side side, CombatRanges combatRange, float stripeSize) {
            SectorInit(side, combatRange, stripeSize);
            tag = "CombatSector";
            SpawnChildSector(sectorSpawner, side, stripeSize);
        }
        // Common sector init
        private void SectorInit(Side side, CombatRanges combatRange, float stripeSize) {
            _sectorData = new SectorData();
            _sectorData.SetPos(side, combatRange);
            Actors = new List<Actor>();
            name = SectorData.SectorName;

            GenerateStrips(stripeSize);
            Debugger.Log($"Sector {name} initialized with size {transform.lossyScale}");
            //Debugger.Watch(new DebugObjectProperties(transform.lossyScale * 0.9f, Color.red, PrimitiveType.Cube));
        }
        // Root sector init
        public void SectorInit(Func<Vector3, Sector, Sector> sectorSpawner, float stripeSize) {

            if (transform.parent != null && transform.parent.GetComponent<Sector>() != null) {
                Debugger.ThrowCriticalError("SectorInit should only be called on the root sector");
            }
            _isRoot = true;
            SectorInit(Side.Middle, CombatRanges.Close, stripeSize);
            tag = "RootCombatSector";
            for (int i = 1; i <= 2; i++) {
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
            Debugger.Log($"Spawning child sector for {name} on side {side}");
            byte lastEnum = Enum.GetValues(typeof(CombatRanges)).Cast<byte>().Max();
            if (lastEnum == (byte)SectorData.GetRange()) {
                return Maybe<Sector>.None();
            }
            Sector s = sectorSpawner(transform.position + Vector3.Scale(transform.lossyScale, Vector3.right) * (side == Side.Left ? -1 : 1), this);
            s.SectorInit(sectorSpawner, side, GetNextRange(SectorData.GetRange()), stripeSize);
            Debugger.Log($"Child sector {s.name} spawned with range {GetNextRange(SectorData.GetRange())}");
            return Maybe<Sector>.Some(s);
        }
        private CombatRanges GetNextRange(CombatRanges currentRange) {
            return Enum.Parse<CombatRanges>(((byte)currentRange << 1).ToString());
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



        public Sector GetSector(SectorPosition sectorPos) {

            Debugger.Log($"Checking sector {name} for side {SectorData.GetSide(sectorPos)} and range {SectorData.GetRange(sectorPos)}; This sector side: {_sectorData.GetSide()} range: {_sectorData.GetRange()}");
            if (SectorData.SectorPosition == sectorPos) {
                return this;
            }
            foreach (Transform child in transform) {
                Sector s = child.GetComponent<Sector>()?.GetSector(sectorPos);
                if (s != null) {
                    return s;
                }
            }
            return null;
        }
    }
}
