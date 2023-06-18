using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Actors {
    [CreateAssetMenu(fileName = "NewPlayerActorData", menuName = "Actors/PlayerActorData")]
    public class PlayerActorData : ActorData {
        [SerializeField]
        float _walkSpeed;
        [SerializeField]
        float _runSpeedModifier;
        public float WalkSpeed { get { return _walkSpeed; } }
        public float RunSpeedModifier { get { return _runSpeedModifier; } }
    }
}
