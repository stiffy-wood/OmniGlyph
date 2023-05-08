using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Items {


    public class Item : ScriptableObject {
        [SerializeField]
        string _name;
        public string Name { get { return _name; } }
    }
}
