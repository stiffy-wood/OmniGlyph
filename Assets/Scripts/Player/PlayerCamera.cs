using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Player {
    public class PlayerCamera : MonoBehaviour {
        public Vector3 CamOffset;
        public float Lag = 0.1f;
        private PlayerMovement _playerMovement;
        private Vector3 _targetPos;

        void Start() {
            _playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            SetTargetPos(_playerMovement.transform.position);
            _playerMovement.PositionChanged += SetTargetPos;
        }

        // Update is called once per frame
        void FixedUpdate() {
            transform.position = Vector3.Lerp(transform.position, _targetPos, Lag);
            transform.LookAt(_playerMovement.transform);
        }

        void SetTargetPos(Vector3 targetPos) {
            _targetPos = targetPos + CamOffset;
        }
    }
}
