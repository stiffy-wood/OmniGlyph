using System;
using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Combat;
using UnityEngine;

namespace OmniGlyph.Actors {
    public class BotActor : Actor, IInteractable {
        protected bool _isFollowingPlayer = false;
        protected bool _wasFollowingPlayer = false;
        [SerializeField]
        private BotCombatAI _combatAI;

        public BotCombatAI CombatAI => _combatAI;
        protected override void Start() {
            base.Start();
            InternalInit();
            InitData();
        }
        protected void InitData() {
            if (_data == null) {
                _data = Resources.Load<ActorData>("Actors/BaseActorData");
            }
            if (_combatData == null) {
                _combatData = Resources.Load<ActorCombatData>("Actors/BaseActorCombatData");
            }
        }
        public override void InternalInit() {
            base.InternalInit();
        }
        protected void OnGameStateChanged(GameStates newState) {
            if (newState != GameStates.Roam) {
                _wasFollowingPlayer = _isFollowingPlayer;
                StopFollowingPlayer();
            } else if (_wasFollowingPlayer) {
                StartFollowingPlayer();
            }
        }
        public virtual void Interact() {
            Debugger.Log($"Interacting with {name}");
        }
        public void StartFollowingPlayer() {
            if (Context.CurrentGameState != GameStates.Roam) {
                return;
            }
            Context.Player.Trail.AddFollower(this);
            _isFollowingPlayer = true;
        }
        public void StopFollowingPlayer() {
            if (Context.CurrentGameState != GameStates.Roam) {
                return;
            }
            Context.Player.Trail.RemoveFollower(this);
            _isFollowingPlayer = false;
        }
    }
}
