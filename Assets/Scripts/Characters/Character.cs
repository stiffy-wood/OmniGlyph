using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Characters {
    public abstract class Character {
        public abstract CharacterInfo Info { get; }
        public abstract void Interact();
    }
}
