using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmniGlyph.Actors;
using OmniGlyph.Actors.Characters;
using OmniGlyph.Internals;
using UnityEngine;

namespace OmniGlyph.Story.Dialogues {
    public class Line {
        public string Text { get; }
        public string CharacterName { get; }
        public Line(string text, string characterName) {
            Text = text;
            CharacterName = characterName;
        }
    }
}
