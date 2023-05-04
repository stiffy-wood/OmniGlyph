using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Internals {
    public interface IInternal {
        public bool IsFinished { get; }
        public InternalsManager InternalsManager { get; set; }
        public void Update();
    }
}
