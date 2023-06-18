using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Combat.Field {
    [Flags]
    public enum SectorPosition : byte {
        Right = 1,
        Left = 2,
        Center = 3,
        Close = 4,
        Mid = 8,
        Far = 16,
    }
}
