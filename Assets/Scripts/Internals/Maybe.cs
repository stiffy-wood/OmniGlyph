using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Internals {
    public class Maybe<T> {
        private readonly T _value;
        private readonly bool _hasValue;
        private Maybe(T value) {
            this._value = value;
            this._hasValue = true;
        }
        private Maybe() {
            this._value = default(T);
            this._hasValue = false;
        }
        public static Maybe<T> Some(T value) {
            return new Maybe<T>(value);
        }
        public static Maybe<T> None() {
            return new Maybe<T>();
        }
        public bool HasValue => _hasValue;
        public T Value {
            get {
                if (!_hasValue) {
                    throw new InvalidOperationException("Doesn not have value");
                }
                return _value;
            }
        }
        public T GetValueOrDefault(T defaultValue) {
            return _hasValue ? _value : defaultValue;
        }
    }
}
