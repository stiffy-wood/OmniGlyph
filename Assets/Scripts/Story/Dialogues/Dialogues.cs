using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Actors.Characters;
using UnityEngine;

namespace OmniGlyph.Story.Dialogues {
    public static class Dialogues {
        public static Dialogue SereonAndBragiIntro = DialogueFactory.CreateDialogue(
            ("(Stay focused, Sereon, you can do this.)", CharacterList.Player),
            ("Sereon! Pay attention, lad. This battle won't fight itself!", CharacterList.KnighCommander),
            ("Y-yes sir!", CharacterList.Player)
        );
    }
}
