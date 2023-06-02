using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Actors;
using OmniGlyph.Combat.Effects;
using UnityEngine;

namespace OmniGlyph.Combat {
    public abstract class Ability {
        public abstract AbilityType Type { get; }
        public abstract float EnergyCost { get; }
        public abstract Effect[] Effects { get; }
        public abstract CombatRanges GetRange(Actor user);
        public abstract bool CanUse(Actor user, Actor target);
        public abstract void Use(Actor user, Actor target);

    }
}
