using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Combat;
using UnityEngine;

namespace OmniGlyph.Combat.Effects {
    public abstract class Effect {
        public abstract void Apply(CombatActor target);
    }
}
