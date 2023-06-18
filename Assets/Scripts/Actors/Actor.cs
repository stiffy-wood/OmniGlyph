using System;
using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Combat;
using OmniGlyph.Combat.Field;
using OmniGlyph.Internals;
using OmniGlyph.Internals.Debugging;
using TMPro;
using UnityEngine;

namespace OmniGlyph.Actors {
    public abstract class Actor : OmniMono {
        [SerializeField]
        protected Vector3 _targetPos;
        [SerializeField]
        protected ActorData _data;
        [SerializeField]
        protected ActorCombatData _combatData;

        public event Action<Vector3> ActorPositionChanged;
        public event Action<Actor> ActorDied;
        protected virtual void Start() {
            SetPosition(transform.position);
        }
        public ActorData Data { get { return _data; } }
        public ActorCombatData CombatData { get { return _combatData; } }
        public Vector3 Size { get { return GetComponent<Collider>().bounds.size * 1.2f; } }
        public void SetPosition(Vector3 targetPos) {
            _targetPos = targetPos;
        }
        protected virtual void FixedUpdate() {
            if (_targetPos - transform.position != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(_targetPos - transform.position);
            }
            transform.position = Vector3.Lerp(transform.position, _targetPos, _data.MovementLag);
        }
        public Maybe<Sector> GetSector() {
            if (Context.CurrentGameState != GameStates.Combat) {
                return Maybe<Sector>.None();
            }
            Maybe<(Sector sector, SectorStrip _)> position = Context.CombatContext.GetActorPosition(this);
            if (position.HasValue) {
                return Maybe<Sector>.Some(position.Value.sector);
            }
            return Maybe<Sector>.None();
        }
    }
}
