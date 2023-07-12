using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using OmniGlyph.Actors;
using OmniGlyph.Combat.Field;
using OmniGlyph.Configs;
using OmniGlyph.Internals;
using OmniGlyph.Internals.Debugging;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

namespace OmniGlyph.Combat {
    public class CombatContext : OmniMonoComponent {
        public Vector3 SectorSize = new Vector3(6, 0, 10);
        public float SectorStripeSize = 1.5f;

        private Sector _rootSector;
        private TurnOrder _turnOrder;
        private Dictionary<Actor, SectorStrip> _actorPositions;

        public SectorStrip SelectedStrip { get; private set; }
        void Start() {
        }

        private void Update() {

        }
        public override void Init(GameContext context) {
            base.Init(context);
            Context.GameStateChanged += OnGameStateChanged;
        }
        private void OnGameStateChanged(GameStates state) {
            Debugger.Log($"{nameof(CombatContext)}Game State Changed: {state}");
            switch (state) {
                case GameStates.Combat:
                    CombatStart();
                    break;
                default:
                    if (_rootSector != null) {
                        CombatEnd();
                    }
                    break;
            }
        }
        private void CombatStart() {
            Actor[] actors = Context.GetActorsInScene();
            Vector3 battlefieldCenter = actors.Select(a => a.transform.position).Aggregate((pos1, pos2) => pos1 + pos2) / actors.Length;

            _rootSector = SpawnSector(battlefieldCenter, null);
            _rootSector.SectorInit(SpawnSector, SectorStripeSize);
            Debugger.Log($"Battlefield center: {battlefieldCenter}\nRoot sector: {_rootSector}\nRoot sector position: {_rootSector.transform.position}");

            PrepareActors(actors);
        }
        private void PrepareActors(Actor[] actors) {
            _actorPositions = new Dictionary<Actor, SectorStrip>();
            foreach (Actor actor in actors) {
                Side combatSide = actor.CombatData.IsOnPlayerSide ? Side.Left : Side.Right;
                Side battlefieldSide = actor.CombatData.StartingRange == CombatRanges.Close ? Side.Middle : combatSide;

                Sector actorSector = _rootSector.GetSector(SectorData.NewSectorPos(battlefieldSide, actor.CombatData.StartingRange));
                Maybe<SectorStrip> actorStrip = actorSector.AddActor(combatSide, actor);
                if (!actorStrip.HasValue) {
                    Debugger.ThrowCriticalError($"Actor {actor} could not be added to sector {actorSector}");
                }
                SetActorInitiave(actor);
                _actorPositions.Add(actor, actorStrip.Value);
            }
            _turnOrder = new TurnOrder(actors.OrderByDescending(a => a.CombatData.Initiative).ToList());
        }
        private void SetActorInitiave(Actor actor) {
            actor.CombatData.Initiative = GetInitiave() + actor.CombatData.Agility;
        }
        private int GetInitiave() {
            return new System.Random().Next(1, 21);
        }
        private void CombatEnd() {
            _rootSector.DestroySector();
            _rootSector = null;
            _turnOrder = null;
        }

        private Sector SpawnSector(Vector3 position, Sector parent) {
            GameObject sectorObject = new GameObject($"Sector{Guid.NewGuid()}");
            sectorObject.transform.position = position;
            sectorObject.transform.localScale = SectorSize;


            if (parent != null) {
                sectorObject.transform.SetParent(parent.transform);
            }
            Sector newSector = sectorObject.AddComponent<Sector>();
            return newSector;
        }

        public void MoveActor(Actor actor, SectorStrip strip) {
            if (!_actorPositions.ContainsKey(actor)) {
                Debugger.ThrowCriticalError($"Actor {actor} is not in the combat context");
            }
            _actorPositions[actor].RemoveActor(actor);
            _actorPositions[actor] = strip;

            strip.SetActor(actor.CombatData.IsOnPlayerSide ? Side.Left : Side.Right, actor);
        }
        public Maybe<SectorStrip> GetActorPosition(Actor actor) {
            if (!_actorPositions.ContainsKey(actor)) {
                return Maybe<SectorStrip>.None();
            }
            return Maybe<SectorStrip>.Some(_actorPositions[actor]);
        }
        public static CombatRanges GetRangeBetweenSectors(Sector sector1, Sector sector2) {
            return SectorData.GetDistance(sector1.SectorData, sector2.SectorData);
        }

        public void OnStripSelect(SectorStrip strip) {
            SelectedStrip = strip;
        }
    }
}
