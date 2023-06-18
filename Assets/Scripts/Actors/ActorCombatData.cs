using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Combat;
using OmniGlyph.Combat.Actions;
using OmniGlyph.Combat.Field;
using OmniGlyph.Items.Armor;
using OmniGlyph.Items.Weapons;
using UnityEngine;

namespace OmniGlyph.Actors {
    [CreateAssetMenu(fileName = "NewActorCombatData", menuName = "Actors/ActorCombatData")]
    public class ActorCombatData : ScriptableObject {
        [SerializeField]
        bool _isOnPlayerSide;
        [SerializeField]
        float _maxHealth;
        [SerializeField]
        float _currentHealth;
        [SerializeField]
        float _strength;
        [SerializeField]
        float _maxEnergy;
        [SerializeField]
        float _energy;
        [SerializeField]
        Weapon _equippedWeapon;
        [SerializeField]
        Armor _equippedArmor;
        [SerializeField]
        Ability[] _abilities;
        [SerializeField]
        CombatRanges _startingRange;
        [SerializeField]
        int _agility;
        [SerializeField]
        int _mind;
        [SerializeField]
        int _initiative;
        public bool IsOnPlayerSide { get { return _isOnPlayerSide; } }
        public float MaxHealth {
            get { return _maxHealth; }
            set { _maxHealth = value; }
        }
        public float CurrentHealth {
            get { return _currentHealth; }
            set { _currentHealth = value; }
        }
        public float Strength {
            get { return _strength; }
            set { _strength = value; }
        }
        public float MaxEnergy {
            get { return _maxEnergy; }
            set { _maxEnergy = value; }
        }
        public float Energy {
            get { return _energy; }
            set { _energy = value; }
        }
        public Weapon EquippedWeapon {
            get { return _equippedWeapon; }
            set { _equippedWeapon = value; }
        }
        public Armor EquippedArmor {
            get { return _equippedArmor; }
            set { _equippedArmor = value; }
        }
        public Ability[] Abilities { get { return _abilities; } }
        public CombatRanges StartingRange {
            get { return _startingRange; }
            set { _startingRange = value; }
        }
        public int Agility {
            get { return _agility; }
            set { _agility = value; }
        }
        public int Mind {
            get { return _mind; }
            set { _mind = value; }
        }
        public int Initiative {
            get { return _initiative; }
            set { _initiative = value; }
        }

    }
}
