using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGlyph.Internals {
    public abstract class OmniMonoComponent : OmniMono {
        public virtual void Init(GameContext context) {
            Context = context;
            Context.GameStateChanged += OnGameStateChange;
        }
        protected virtual void OnGameStateChange(GameStates newGameState) {
            // do nothing
        }
    }
}
