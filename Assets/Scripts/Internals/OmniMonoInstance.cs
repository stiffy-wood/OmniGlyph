using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OmniGlyph.Internals {
    public abstract class OmniMonoInstance : OmniMono {
        public virtual void Init() {
            Context = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameContext>();
            Context.GameStateChanged += OnGameStateChanged;
        }
        protected virtual void OnGameStateChanged(GameStates newGameState) {
            // do nothing
        }
    }
}
