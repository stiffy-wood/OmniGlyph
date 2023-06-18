using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Actors {
    [CreateAssetMenu(fileName = "NewActorData", menuName = "Actors/ActorData")]
    public class ActorData : ScriptableObject {

        [SerializeField]
        float _movementLag;

        public float MovementLag {
            get { return _movementLag; }
        }
    }
}
