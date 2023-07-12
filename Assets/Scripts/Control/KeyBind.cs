using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Control {
    [Serializable]
    public struct KeyBind {
        public ControlActions name;
        public KeyCode key;
    }
}
