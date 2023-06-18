using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniGlyph.Combat.Field;

namespace OmniGlyph.Combat.Actions {
    public class MoveAction : CombatAction {
        public override void Execute(ActionData actionData) {
            if (!actionData.TargetStrip.HasValue) {
                throw new ArgumentException("MoveAction requires a target strip");
            }
            actionData.CombatContext.MoveActor(
                actionData.Owner,
                actionData.TargetStrip.Value.ParentSector,
                actionData.TargetStrip.Value
                );
        }
    }
}
