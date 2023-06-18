using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using OmniGlyph.Actors;
using OmniGlyph.Combat.Effects;
using OmniGlyph.Combat.Field;
using OmniGlyph.Internals;
using UnityEngine;

namespace OmniGlyph.Combat.Actions.Abilities {
    public class SimpleAttack : Ability {
        public override AbilityType Type => AbilityType.Attack;

        public override float EnergyCost => 0f;

        public override Effect[] Effects => null;
        public override CombatRanges GetRange(Actor user) {
            return user.CombatData.EquippedWeapon.CombatRange;
        }
        public override bool CanUse(Actor user, Actor target) {
            Sector userSector = user.GetSector().GetValueOrDefault(null);
            Sector targetSector = user.GetSector().GetValueOrDefault(null);
            if (userSector == null || targetSector == null) {
                return false;
            }
            CombatRanges actualRange = CombatContext.GetRangeBetweenSectors(userSector, targetSector);
            CombatRanges neededRange = GetRange(user);
            return (short)actualRange > (short)neededRange;

        }
        public override void Use(Actor user, Actor target) {
            if (!CanUse(user, target)) {
                throw new System.Exception($"Cannot use the fucking ability {nameof(SimpleAttack)}");
            }
            //todo: make it possible to use different kind of bonus, in cases where the simple attack is not strength based
            float damage = user.CombatData.EquippedWeapon.Damage + user.CombatData.Strength;

            //todo: invoke actor animations
            target.CombatData.CurrentHealth -= damage;
        }
    }
}
