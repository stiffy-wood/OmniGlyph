using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Items.Armor {
    [CreateAssetMenu(fileName = "NewArmor", menuName = "Items/Armor")]
    public class Armor : Item {
        [SerializeField]
        float _defense; // Amount of damage it can block before breaking
        public float Defense { get { return _defense; } }
    }
}
