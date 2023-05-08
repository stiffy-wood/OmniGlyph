using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Combat;
using UnityEngine;

namespace OmniGlyph.Player {
    public class PlayerCamera : MonoBehaviour {
        public Vector3 CamOffset;
        public float Lag = 0.1f;
        public Vector3 CamBattlefieldOffset;
        private IActor _playerMovement;
        private Vector3 _targetPos;
        private bool _combatMode = false;
        private Vector3 _battlefieldPos;
        private Vector3 _lookTarget;
        void Start() {
            _playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            SetTargetPos(_targetPos);
            _playerMovement.PositionChanged += SetTargetPos;

            BattleController controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<BattleController>();
            controller.CombatStarted += p => { _combatMode = true; _battlefieldPos = p; };
            controller.CombatEnded += () => { _combatMode = false; _battlefieldPos = default; };
        }

        void FollowPlayer() {
            transform.position = Vector3.Lerp(transform.position, _targetPos, Lag);
            SetLookTarget(_playerMovement.GetPosition());
        }
        void LookAtBattlefield() {
            transform.position = Vector3.Lerp(transform.position, _battlefieldPos + CamBattlefieldOffset, Lag);
            SetLookTarget(_battlefieldPos);
        }
        void FixedUpdate() {
            if (!_combatMode) {
                FollowPlayer();
            } else {
                LookAtBattlefield();
            }
            transform.LookAt(_lookTarget);
        }

        void SetTargetPos(Vector3 targetPos) {
            _targetPos = targetPos + CamOffset;
        }
        void SetLookTarget(Vector3 targetPos) {
            _lookTarget = Vector3.Lerp(_lookTarget, targetPos, Lag);
        }
    }
}
