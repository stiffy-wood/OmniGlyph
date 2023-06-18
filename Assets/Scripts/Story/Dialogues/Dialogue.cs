using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Story.Dialogues {
    public class Dialogue : IEnumerator {
        private Line[] _lines;
        private int _currentIndex;
        public Dialogue(Line[] lines) {
            _lines = lines;
            _currentIndex = -1;
        }
        public object Current => _lines[_currentIndex];
        public bool MoveNext() {
            _currentIndex++;

            return _currentIndex < _lines.Length;
        }
        public void Reset() {
            _currentIndex = -1;
        }
    }
}
