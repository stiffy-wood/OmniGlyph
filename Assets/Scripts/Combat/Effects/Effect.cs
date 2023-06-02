using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Actors;
using OmniGlyph.Combat;
using UnityEngine;

namespace OmniGlyph.Combat.Effects {
    public abstract class Effect {
        public abstract void Apply(Actor target);
    }
}
