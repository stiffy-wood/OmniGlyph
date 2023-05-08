using System;
using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Combat;
using UnityEngine;

namespace OmniGlyph {
    public interface IActor {
        public CombatActor CombatActor { get; }
        public event Action<Vector3> PositionChanged;
        public event Action<IActor> ActorDied;
        public Vector3 GetPosition();
        public void SetPosition(Vector3 targetPos);
        public Vector3 GetSize();
    }
}
