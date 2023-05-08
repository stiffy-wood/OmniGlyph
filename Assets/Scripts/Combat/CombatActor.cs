using System;
using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Bots;
using OmniGlyph.Combat.Field;
using OmniGlyph.Configs;
using OmniGlyph.Internals;
using OmniGlyph.Internals.Debugging;
using OmniGlyph.Items.Armor;
using OmniGlyph.Items.Weapons;
using TMPro;
using UnityEditor;
using UnityEngine;
namespace OmniGlyph.Combat {
    public class CombatActor {
        public Sector currentSector;
        public CombatRanges startingRange;
        public IActor ParentActor {
            get { return _parentActor; }
        }
        public bool IsOnPlayerSide {
            get { return _parentActor is not Enemy; }
        }

        [SerializeField]
        Weapon _equippedWeapon;
        [SerializeField]
        Armor _equippedArmor;

        public Weapon EquippedWeapon {
            get { return _equippedWeapon; }
            set { _equippedWeapon = value; }
        }
        public Armor EquippedArmor {
            get { return _equippedArmor; }
            set { _equippedArmor = value; }
        }
        public Ability[] Abilities { get; set; }

        private int _health;
        private int _maxHealth;
        private int _energy;
        private int _maxEnergy;
        private int _strength;

        private IActor _parentActor;
        private InternalDebugger _debugger;

        public CombatActor(int maxHealth, int maxEnergy, int strength, IActor parent) {
            _maxHealth = maxHealth;
            _maxEnergy = maxEnergy;
            _strength = strength;

            _health = _maxHealth;
            _energy = _maxEnergy;

            _parentActor = parent;

            _debugger = GameObject.FindGameObjectWithTag("GameController").GetComponent<InternalsManager>().Get<InternalDebugger>();

        }

        public void MoveLeft() {
            Move(Side.Left);
        }
        public void MoveRight() {
            Move(Side.Right);
        }
        private void Move(Side direction) {
            Vector3 oldCenter = currentSector.Center;
            Vector3 newCenter = currentSector.TransferCombatActor(this, direction);
            if (newCenter == oldCenter) {
                _debugger.Log($"Unable to move {this} CombatActor in {direction} direction from sector {currentSector.CombatRange} {currentSector.Side}");
                return;
            }
            Vector3 actorPos = _parentActor.GetPosition();
            actorPos.Set(newCenter.x, actorPos.y, actorPos.z);

            _parentActor.SetPosition(actorPos);
        }

    }
}
