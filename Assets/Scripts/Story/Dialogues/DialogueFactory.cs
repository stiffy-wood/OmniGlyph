using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Story.Dialogues {
    public static class DialogueFactory {
        public static Dialogue CreateDialogue(params (string, string)[] lines) {
            List<Line> lineList = new List<Line>();
            foreach (var line in lines) {
                lineList.Add(new Line(line.Item1, line.Item2));
            }
            return new Dialogue(lineList.ToArray());
        }
    }
}
