using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using OmniGlyph.Combat.Field;
using OmniGlyph.Configs;
using OmniGlyph.Internals;
using OmniGlyph.Internals.Debugging;
using OmniGlyph.Player;
using UnityEngine;
namespace OmniGlyph.Combat {
    public class BattleController : MonoBehaviour {
        public Vector3 SectorSize = new Vector3(6, 0, 10);

        public event Action<Vector3> CombatStarted;
        public event Action CombatEnded;

        private Sector _sectorsRoot;
        private ConcurrentQueue<Action> _combatActions;
        private InternalDebugger _debugger;
        void Start() {
            _combatActions = new ConcurrentQueue<Action>();
            InternalsManager im = gameObject.GetComponent<InternalsManager>();
            _debugger = im.Get<InternalDebugger>();
        }

        private void Update() {

        }

        public void CombatStart(List<CombatActor> combatActors) {
            Vector3 battlefieldCenter = combatActors.Select(a => a.ParentActor.GetPosition()).Aggregate((pos1, pos2) => pos1 + pos2) / combatActors.Count;
            _sectorsRoot = new Sector(SectorSize, battlefieldCenter);
            MoveCombatActorsToPos(combatActors);
            CombatStarted?.Invoke(_sectorsRoot.Center);
        }
        public void CombatEnd() {
            CombatEnded?.Invoke();
            _sectorsRoot.Destroy();
            _sectorsRoot = null;
        }
        public void MoveCombatActorsToPos(List<CombatActor> combatActors) {
            foreach (CombatActor actor in combatActors) {
                _debugger.Log($"Placing {actor} - {actor.startingRange}");

                int direction = actor.IsOnPlayerSide ? 0 : 1;
                Sector currentSector = _sectorsRoot;
                while (actor.startingRange != currentSector.CombatRange) {
                    currentSector = currentSector.Children[direction];
                    direction = 0;
                }
                _debugger.Log($"Placed actor in {CombatRanges.Close}");
                currentSector.PlaceCombatActor(actor);
            }
        }

    }
}
