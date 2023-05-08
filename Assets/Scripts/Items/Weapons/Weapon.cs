using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Combat;
using OmniGlyph.Combat.Effects;
using UnityEngine;

namespace OmniGlyph.Items.Weapons {
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
    public class Weapon : Item {
        [SerializeField]
        float _damage;
        [SerializeField]
        Effect[] _effects;
        [SerializeField]
        CombatRanges _combatRange;
        public float Damage { get { return _damage; } }
        public Effect[] Effects { get { return _effects; } }
        public CombatRanges CombatRange { get { return _combatRange; } }


    }
}
