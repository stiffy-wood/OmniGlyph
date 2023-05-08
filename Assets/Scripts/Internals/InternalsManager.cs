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
                @internal.Value.InternalUpdate();
            }
            foreach (var @internal in internalsToRemove) {
                _internals.TryRemove(@internal, out var _);
            }
        }


        public T Get<T>() where T : IInternal {
            if (typeof(BaseMonoInternal).IsAssignableFrom(typeof(T))) {
                throw new ArgumentException("Use GetMono<T> for MonoInternals");
            }
            if (!_internals.TryGetValue(typeof(T), out IInternal @internal)) {
                @internal = InstantiateInternal<T>();
                _internals.TryAdd(typeof(T), @internal);
            }
            return (T)@internal;
        }
        public T GetMono<T>() where T : BaseMonoInternal {
            if (!_internals.TryGetValue(typeof(T), out IInternal @internal)) {
                @internal = InstantiateMonoInternal<T>();
                _internals.TryAdd(typeof(T), @internal);
            }
            return (T)@internal;
        }

        private T InstantiateInternal<T>() where T : IInternal {
            T @internal = (T)Activator.CreateInstance(typeof(T));
            @internal.Init(this);
            return @internal;
        }
        private T InstantiateMonoInternal<T>() where T : BaseMonoInternal {
            T @internal = gameObject.AddComponent<T>();
            @internal.Init(this);
            return @internal;
        }
    }
}
