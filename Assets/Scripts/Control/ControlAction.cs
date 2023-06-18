using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniGlyph.AControl;
using UnityEngine;

namespace OmniGlyph.Control {
    public class ControlAction {
        private KeyCode _key;
        private Action _action;
        private ButtonActivations _activation;
        private GameContext _context;
        public ControlAction(KeyCode key, Action action, ButtonActivations activation, GameContext context) {
            _key = key;
            _action = action;
        }

        public void Do(GameContext currentContext) {
            if (IsActivated(currentContext)) {
                _action();
            }
        }
        private bool IsActivated(GameContext currentContext) {
            if (currentContext != _context)
                return false;

            switch (_activation) {
                case ButtonActivations.Up:
                    return Input.GetKeyUp(_key);
                case ButtonActivations.Down:
                    return Input.GetKeyDown(_key);
                case ButtonActivations.Hold:
                    return Input.GetKey(_key);
                default:
                    return false;
            }
        }
    }
}
