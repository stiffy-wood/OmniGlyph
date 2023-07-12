using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace OmniGlyph.UI {
    public record TextProperties {
        public string Text { get; set; }
        public Color Color { get; set; }
        public int FontSize { get; set; }
        public TextAlignmentOptions Alignment { get; set; }
        public TextProperties() {

        }
    }
}
