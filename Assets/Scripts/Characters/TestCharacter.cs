using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Characters {
    public class TestCharacter : Character {
        public override CharacterInfo Info { get; }

        public override void Interact() {
            throw new System.NotImplementedException();
        }
    }
}
