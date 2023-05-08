using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Internals {
    public interface IInternal {
        public bool IsFinished { get; }
        public void InternalUpdate();
        public void Init(InternalsManager manager);
    }
}
