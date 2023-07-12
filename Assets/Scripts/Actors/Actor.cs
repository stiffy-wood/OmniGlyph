using System;
using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Combat;
using OmniGlyph.Combat.Field;
using OmniGlyph.Internals;
using OmniGlyph.Internals.Debugging;
using OmniGlyph.UI;
using TMPro;
using UnityEngine;

namespace OmniGlyph.Actors {
    public abstract class Actor : OmniMonoUI {
        [SerializeField]
        protected Vector3 _targetPos;
        [SerializeField]
        protected ActorData _data;
        [SerializeField]
        protected ActorCombatData _combatData;

        public event Action<Vector3> ActorPositionChanged;
        public event Action<Actor> ActorDied;
        protected override void Start() {
            base.Start();
            SetPosition(transform.position);
            Init();
        }
        public override void Init() {
            base.Init();
            ShowProfilePic();
        }
        protected override void OnGameStateChanged(GameStates newGameState) {
            base.OnGameStateChanged(newGameState);
            HideOverheadText();
            switch (newGameState) {
                case GameStates.Menu:
                    HideProfilePic();
                    break;
                default:
                    ShowProfilePic();
                    break;
            }
        }
        public ActorData Data { get { return _data; } }
        public ActorCombatData CombatData { get { return _combatData; } }
        public Vector3 Size { get { return GetComponent<Collider>().bounds.size * 1.2f; } }
        public void SetPosition(Vector3 targetPos) {
            _targetPos = targetPos;
        }
        public void SetRotation(Quaternion rotation) {
            transform.rotation = rotation;
        }
        protected void FixedUpdate() {
            if (_targetPos - transform.position != Vector3.zero) {
                SetRotation(Quaternion.LookRotation(_targetPos - transform.position));
            }
            transform.position = Vector3.Lerp(transform.position, _targetPos, _data.MovementLag);
        }
        public Maybe<SectorStrip> GetSectorStrip() {
            if (!GameContext.IsGameStateEqual(Context.CurrentGameState, GameStates.Combat)) {
                return Maybe<SectorStrip>.None();
            }
            Maybe<SectorStrip> position = Context.CombatContext.GetActorPosition(this);
            if (position.HasValue) {
                return Maybe<SectorStrip>.Some(position.Value);
            }
            return Maybe<SectorStrip>.None();
        }
        protected void ShowProfilePic() {
            if (_data.ProfilePic != null) {
                ShowOverheadImage(_data.ProfilePic);
            }
        }
        protected void HideProfilePic() {
            HideOverheadImage();
        }
    }
}
