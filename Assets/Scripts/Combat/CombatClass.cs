using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Combat.Actions {
    [CreateAssetMenu(fileName = "CombatClass", menuName = "Combat/CombatClass")]
    public class CombatClass : ScriptableObject {
        [SerializeField]
        Ability[] _abilites;
        public Ability[] Abilities { get { return _abilites; } set { _abilites = value; } }
        public string Name { get; }
        public int StrengthMod { get; }
        public int HealthMod { get; }
        public int EnergyMod { get; }
        public int AgilityMod { get; }
        public int MindMod { get; }
        public CombatRanges DefaultRange { get; }
    }
}
