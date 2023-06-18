using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Internals.Debugging {

    public record DebugObjectProperties {
        public Vector3 Size { get; }
        public Color Color { get; }
        public PrimitiveType Type { get; }
        public DebugObjectProperties(Vector3 size = default, Color color = default, PrimitiveType type = PrimitiveType.Sphere) {
            Size = size;
            Color = color;
            Type = type;
        }
    }
}