using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Combat.Effects;
using UnityEngine;

namespace OmniGlyph.Combat {
    public abstract class Ability {
        public abstract float EnergyCost { get; }
        public abstract Effect[] Effects { get; }
        public abstract void Use();

    }
}
