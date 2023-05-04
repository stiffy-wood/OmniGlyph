using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Internals {
    public class InternalsManager : MonoBehaviour {

        ConcurrentDictionary<Type, IInternal> _internals = new ConcurrentDictionary<Type, IInternal>();

        void Start() {

        }


        void Update() {
            List<Type> internalsToRemove = new List<Type>();
            foreach (var @internal in _internals) {
                if (@internal.Value.IsFinished) {
                    internalsToRemove.Add(@internal.Key);
                    continue;
                }
                @internal.Value.Update();
            }
            foreach (var @internal in internalsToRemove) {
                _internals.TryRemove(@internal, out var _);
            }
        }


        public T Get<T>() where T : IInternal {
            if (!_internals.TryGetValue(typeof(T), out IInternal @internal)) {
                @internal = (T)Activator.CreateInstance(typeof(T));
                @internal.InternalsManager = this;
                _internals.TryAdd(typeof(T), @internal);
            }
            return (T)@internal;
        }
        public T GetNew<T>() where T : IInternal {
            T @internal = (T)Activator.CreateInstance(typeof(T));
            @internal.InternalsManager = this;
            return @internal;
        }

    }
}
