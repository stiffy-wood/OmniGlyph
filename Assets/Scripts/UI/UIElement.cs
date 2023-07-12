using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OmniGlyph.UI {
    public record UIElement {
        public GameObject ParentObject { get; set; }
        public GameObject TextObject { get; set; }
        public Vector3 Offset { get; set; }
        public bool ShouldDispose { get; set; }
        public UIElement() {
        }
        public void Dispose() {
            ShouldDispose = true;
        }
    }
}
