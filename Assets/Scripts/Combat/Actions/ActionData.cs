using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniGlyph.Actors;
using OmniGlyph.Combat.Field;
using OmniGlyph.Internals;

namespace OmniGlyph.Combat.Actions {
    public record ActionData {
        public Actor Owner { get; }
        public CombatContext CombatContext { get; }
        public Maybe<Actor> TargetActor { get; }
        public Maybe<SectorStrip> TargetStrip { get; }

        public ActionData(Actor owner, Actor targetActor) {
            Owner = owner;
            CombatContext = owner.Context.CombatContext;
            TargetActor = Maybe<Actor>.Some(targetActor);
            TargetStrip = Maybe<SectorStrip>.None();
        }

        public ActionData(Actor owner, SectorStrip targetSectorStrip) {
            Owner = owner;
            CombatContext = owner.Context.CombatContext;
            TargetActor = Maybe<Actor>.None();
            TargetStrip = Maybe<SectorStrip>.Some(targetSectorStrip);
        }

        public ActionData(Actor owner) {
            Owner = owner;
            CombatContext = owner.Context.CombatContext;
            TargetActor = Maybe<Actor>.None();
            TargetStrip = Maybe<SectorStrip>.None();
        }

    }
}
