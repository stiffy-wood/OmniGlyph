using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OmniGlyph.Internals.Events {
    public record MultiCondition : ICondition {
        private ICondition[] _conditions;
        public MultiCondition(params ICondition[] conditions) {
            _conditions = conditions;
        }
        public bool Is() {
            return _conditions.All(condition => condition.Is());
        }
    }
}
