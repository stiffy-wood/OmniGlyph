using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Actors.Characters {
    public class BetaMcBuggerson : BotActor {
        public override void Interact() {
            base.Interact();
            Context.CurrentGameState = GameStates.Combat;
        }
    }
}
