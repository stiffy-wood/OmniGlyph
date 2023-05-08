using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OmniGlyph.Characters {
    public record CharacterInfo {
        public string Name { get; set; }

        private string _stringResourcePrefix {
            get {
                return Name.Split(' ').Select(x => x.ToLower()).Aggregate((x, y) => $"{x}_{y}");
            }
        }
        private int _lastConvoSuffix = 1;
        private int _lastLineSuffix = 1;

    }
}
