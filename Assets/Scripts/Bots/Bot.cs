using System;
using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Combat;
using UnityEngine;

namespace OmniGlyph.Bots {
    public abstract class Bot : MonoBehaviour, IActor {
        public float MovementLag = 0.3f;
        CombatActor _combatActor;
        public CombatActor CombatActor { get { return _combatActor; } }

        public event Action<Vector3> PositionChanged;
        public event Action<IActor> ActorDied;

        public Vector3 GetPosition() {
            return transform.position;
        }
        public void SetPosition(Vector3 targetPos) {
            _targetPos = targetPos;
        }
        public Vector3 GetSize() {
            return GetComponent<Collider>().bounds.size;
        }
        Vector3 _targetPos;

        // Start is called before the first frame update
        void Start() {

        }

        void FixedUpdate() {
            transform.position = Vector3.Lerp(transform.position, _targetPos, MovementLag);
        }
    }
}
