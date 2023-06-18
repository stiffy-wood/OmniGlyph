using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Actors;
using OmniGlyph.Internals;
using UnityEngine;

namespace OmniGlyph.Combat {
    public class TurnOrder {
        private List<Actor> _actors;
        private int _currentActorIndex = 0;

        public TurnOrder(List<Actor> actors) {
            _actors = actors;
        }
        public Maybe<Actor> GetCurrentActor() {
            if (_actors.Count == 0) {
                return Maybe<Actor>.None();
            }
            return Maybe<Actor>.Some(_actors[_currentActorIndex]);
        }
        public void MoveTurnOrder() {
            if (_currentActorIndex == _actors.Count - 1) {
                _currentActorIndex = 0;
            } else {
                _currentActorIndex++;
            }
        }
        public Maybe<Actor> GetActorAndMoveTurnOrder() {
            Maybe<Actor> a = GetCurrentActor();
            MoveTurnOrder();
            return a;
        }
    }


}
