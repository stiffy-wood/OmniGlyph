using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGlyph.Combat.Actions {
    public abstract class CombatAction {
        public virtual void Execute(ActionData actionData) {
            // do nothing
        }



    }
}
