using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Internals;
using UnityEngine;

namespace OmniGlyph.Actors {
    public class BotCombatAI : OmniMono {
        [SerializeField]
        private Actor _target;

        public Actor Target { get { return _target; } set { _target = value; } }
    }
}
