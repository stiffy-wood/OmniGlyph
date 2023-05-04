using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph {
    public interface IICondition {
        public bool Is();
        public bool Equals(object obj);
        public int GetHashCode();
    }
}
