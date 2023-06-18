using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Combat;
using UnityEngine;

namespace OmniGlyph.Story {
    public record PlayerBaseClass {
        public static PlayerBaseClass Swordsman = new("Swordsman", 2, 0, 0, 0, 0, CombatRanges.Close);
        public static PlayerBaseClass Archer = new("Archer", 1, 0, 1, 1, 0, CombatRanges.Far);
        public static PlayerBaseClass Vanguard = new("Vanguard", 0, 2, 0, -1, 2, CombatRanges.Close);
        public static PlayerBaseClass Skirmisher = new("Skirmisher", 0, 0, 2, 2, 0, CombatRanges.Mid);
        public string Name { get; }
        public int StrengthMod { get; }
        public int HealthMod { get; }
        public int EnergyMod { get; }
        public int AgilityMod { get; }
        public int MindMod { get; }
        public CombatRanges DefaultRange { get; }
        private PlayerBaseClass(
            string name,
            int strengthMod,
            int healthMod,
            int energyMod,
            int abilityMod,
            int mindMod,
            CombatRanges defaultRange) {
            Name = name;
            StrengthMod = strengthMod;
            HealthMod = healthMod;
            EnergyMod = energyMod;
            AgilityMod = abilityMod;
            MindMod = mindMod;
            DefaultRange = defaultRange;
        }
    }
}
