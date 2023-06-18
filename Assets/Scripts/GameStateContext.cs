using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Cam;
using OmniGlyph.Control;
using UnityEngine;

namespace OmniGlyph {
    [CreateAssetMenu(fileName = "NewGameStateContext", menuName = "Contexts/GameStateContext")]
    public class GameStateContext : ScriptableObject {
        [SerializeField]
        ContextKeyBinds _keybinds;
        [SerializeField]
        CameraContext _cameraContext;
        public ContextKeyBinds KeyBinds => _keybinds;
        public CameraContext CameraContext => _cameraContext;
    }
}
