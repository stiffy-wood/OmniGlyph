using System;
using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Actors.Characters;
using OmniGlyph.Combat;
using OmniGlyph.Combat.Actions;
using OmniGlyph.Configs;
using OmniGlyph.Control;
using OmniGlyph.Internals;
using OmniGlyph.Internals.Debugging;
using OmniGlyph.UI;
using UnityEngine;
namespace OmniGlyph.Actors {
    public class PlayerActor : Actor {
        [SerializeField]
        private InputManager _input;
        private InputManager Input {
            get {
                if (_input == null) {
                    _input = Context.InputManager;
                }
                return _input;
            }
        }
        protected override void Start() {
            base.Start();
            Trail = new PlayerFollowLine();
            name = CharacterList.Player;
        }
        public new PlayerActorData Data { get { return (PlayerActorData)_data; } }
        public PlayerFollowLine Trail { get; private set; }
        private void Update() {
            if (Input == null) {
                return;
            }
            Maybe<Vector3> newTargetPos = _input.GetPlayerMovement();
            Maybe<bool> isSprinting = _input.IsPlayerSprinting();
            _targetPos += newTargetPos.GetValueOrDefault(Vector3.zero) * Data.WalkSpeed * (isSprinting.GetValueOrDefault(false) ? Data.RunSpeedModifier : 1);

            if (Context.CurrentGameState == GameStates.Roam) {
                Maybe<IInteractable> interactable = CheckInteract();
                if (interactable.HasValue && _input.IsPlayerInteracting().GetValueOrDefault(false)) {
                    interactable.Value.Interact();
                }
                Trail.AddPoint(transform.position);
            }
            if (Context.CurrentGameState == GameStates.Combat) {
                TestCombat();
            }

        }

        private Maybe<IInteractable> CheckInteract() {
            RaycastHit hit;
            if (Physics.BoxCast(transform.position, Vector3.one / 2f, transform.forward, out hit, transform.rotation, 2f)) {
                GameObject hitObject = hit.collider.gameObject;
                IInteractable interactable = hitObject.GetComponent<IInteractable>();
                if (interactable != null) {
                    Context.UIContext.ShowInteractOption();
                    return Maybe<IInteractable>.Some(interactable);
                }
            }
            Context.UIContext.HideInteractOption();
            return Maybe<IInteractable>.None();
        }

        private void TestCombat() {
            Maybe<bool> isPressed = Context.InputManager.IsTestButtonPressed();
            if (!isPressed.HasValue) {
                return;
            }
            if (Context.CombatContext.SelectedStrip == null) {
                return;
            }
            ActionData data = new ActionData(this, Context.CombatContext.SelectedStrip);
            MoveAction action = new MoveAction();
            action.Execute(data);
        }


    }
}