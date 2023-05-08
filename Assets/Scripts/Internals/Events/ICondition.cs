using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Internals.Events {
    public interface ICondition {
        public bool Is();
    }
}
