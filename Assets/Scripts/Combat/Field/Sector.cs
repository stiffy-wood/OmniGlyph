using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmniGlyph.Configs;
using OmniGlyph.Internals;
using OmniGlyph.Internals.Debugging;
using UnityEngine;

namespace OmniGlyph.Combat.Field {
    public class Sector {
        public List<CombatActor> combatActors { get; private set; }
        public Vector3 Center {
            get {
                return (_boundingBox.Item1 + _boundingBox.Item2) / 2f;
            }
        }
        public static event Action<Sector, Sector, CombatActor> ActorMoved;
        public CombatRanges CombatRange { get; set; }
        public Side Side { get; }
        private Vector3 _dimensions;
        public Vector3 Dimensions {
            get {
                return _dimensions;
            }
        }
        public Sector[] Children {
            get {
                return _children;
            }
        }
        private Sector _parent;
        private Sector[] _children = new Sector[] { };

        private Tuple<Vector3, Vector3> _boundingBox;
        private InternalDebugger _debugger;
        public Sector(Vector3 dimensons, Vector3 center, Sector parent, Side side, CombatRanges combatRange) {
            Init(dimensons, center, parent);
            CombatRange = combatRange;
            Side = side;

            short lastEnum = Enum.GetValues(typeof(CombatRanges)).Cast<short>().Max();
            if (lastEnum != (short)CombatRange) {
                CombatRanges furtherEnum = Enum.Parse<CombatRanges>(((short)CombatRange << 1).ToString());
                _children = new Sector[] {
                new Sector(_dimensions, Center + ((Side == Side.Left) ? Vector3.left : Vector3.right) * _dimensions.x, this, Side, furtherEnum), // TODO: The increasing of combat ranges ain't working
                };
            }
        }
        public Sector(Vector3 dimensions, Vector3 center) {
            Init(dimensions, center, null);
            CombatRange = CombatRanges.Close;
            Side = Side.Middle;

            _children = new Sector[] {
            new Sector(_dimensions, Center - (Vector3.right * _dimensions.x), this, Side.Left, (CombatRanges)((short)CombatRange << 1)),
            new Sector(_dimensions, Center + (Vector3.right * _dimensions.x), this, Side.Right, (CombatRanges)((short)CombatRange << 1)),
            };
        }
        private void Init(Vector3 dimensions, Vector3 center, Sector parent) {
            _dimensions = dimensions;
            _parent = parent;
            _boundingBox = new Tuple<Vector3, Vector3>(
                new Vector3(center.x - (_dimensions.x / 2), center.y, center.z - (_dimensions.z / 2)),
                new Vector3(center.x + (_dimensions.x / 2), center.y, center.z + (_dimensions.z / 2))
            );
            combatActors = new List<CombatActor>();

            GameObject controller = GameObject.FindGameObjectWithTag("GameController");
            InternalsManager im = controller.GetComponent<InternalsManager>();
            BattleController bc = controller.GetComponent<BattleController>();

            _debugger = im.Get<InternalDebugger>();
            _debugger.StartVisualizingSector(a => bc.CombatEnded += a, this);
            _debugger.Log($"Sector created with bounding box {_boundingBox.Item1} to {_boundingBox.Item2}\n Center: {Center}");

        }
        public void Destroy() {
            foreach (Sector child in _children) {
                child.Destroy();
            }
            _debugger.Log($"Sector destroyed with bounding box {_boundingBox.Item1} to {_boundingBox.Item2}\n Center: {Center}");

        }
        public Sector GetRoot() {
            if (_parent == null) {
                return this;
            }
            return _parent.GetRoot();
        }
        public Vector3 TransferCombatActor(CombatActor targetActor, Side moveDirection) {
            if (!combatActors.Contains(targetActor) ||
                (_children.Length == 0 && moveDirection == Side)) {
                return Center;
            }
            if (Side == Side.Middle) {
                switch (moveDirection) {
                    case Side.Left:
                        return TransferCombatActor(targetActor, _children[0]);
                    case Side.Right:
                        return TransferCombatActor(targetActor, _children[1]);
                    default:
                        return Center;
                }
            } else if (Side != moveDirection) {
                return TransferCombatActor(targetActor, _parent);
            } else {
                return TransferCombatActor(targetActor, _children[0]);
            }
        }
        private Vector3 TransferCombatActor(CombatActor targetActor, Sector targetSector) {
            Vector3 placedPosition = targetSector.PlaceCombatActor(targetActor);
            combatActors.Remove(targetActor);
            ActorMoved?.Invoke(this, targetSector, targetActor);
            return placedPosition;
        }

        public Vector3 PlaceCombatActor(CombatActor combatActor) {
            combatActors.Add(combatActor);
            Vector3 targetPosition = Center;
            if (combatActor.IsOnPlayerSide) {
                targetPosition = GetHalfCenter(Side.Left);
            } else {
                targetPosition = GetHalfCenter(Side.Right);
            }
            // TODO: Add a case when there are multiple actors in the same sector
            combatActor.ParentActor.SetPosition(targetPosition);
            return targetPosition;
        }
        private Vector3 GetHalfCenter(Side side) {
            switch (side) {
                case Side.Left:
                    return Center - (Vector3.right * (_dimensions.x / 4));
                case Side.Right:
                    return Center + (Vector3.right * (_dimensions.x / 4));
                default:
                    return Center;
            }
        }
    }
}
