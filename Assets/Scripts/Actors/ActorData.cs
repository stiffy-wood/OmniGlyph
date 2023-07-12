using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Actors {
    [CreateAssetMenu(fileName = "NewActorData", menuName = "Actors/ActorData")]
    public class ActorData : ScriptableObject {

        [SerializeField]
        float _movementLag;
        [SerializeField]
        string _name;
        [SerializeField]
        Texture2D _profilePic;
        public float MovementLag {
            get { return _movementLag; }
        }
        public string Name {
            get { return _name; }
        }
        public Texture2D ProfilePic {
            get { return _profilePic; }
        }
    }
}
