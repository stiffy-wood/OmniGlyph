using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Internals {
    public class TempStorage : IInternal {
        private class StoredValue {
            public object value;
            public DateTime lastAccessTime;
            public StoredValue(object value) {
                this.value = value;
                lastAccessTime = DateTime.Now;
            }
        }
        private Dictionary<Guid, StoredValue> _collection = new Dictionary<Guid, StoredValue>();
        private TimeSpan _storedValueLifetime = TimeSpan.MaxValue;
        private DateTime _lastLifetimeCheck;
        public bool IsFinished { get; set; }

        private InternalsManager _manager;
        public void Init(InternalsManager manager) {
            _manager = manager;
            _lastLifetimeCheck = DateTime.Now;
        }

        public void InternalUpdate() {
            if (DateTime.Now - _lastLifetimeCheck < _storedValueLifetime) {
                return;
            }
            foreach (KeyValuePair<Guid, StoredValue> kvp in _collection) {
                if (DateTime.Now - kvp.Value.lastAccessTime > _storedValueLifetime) {
                    _collection.Remove(kvp.Key);
                }
            }
        }
        public Guid Set(object value) {
            Guid address = Guid.NewGuid();
            _collection.Add(address, new StoredValue(value));
            return address;
        }
        public void Set(object value, Guid address) {
            if (!_collection.ContainsKey(address)) {
                _collection.Add(address, new StoredValue(value));
            } else {
                _collection[address].value = value;
                _collection[address].lastAccessTime = DateTime.Now;
            }
        }
        public T Get<T>(Guid address) {
            if (!_collection.ContainsKey(address)) {
                return default(T);
            }
            StoredValue foundValue = _collection[address];
            foundValue.lastAccessTime = DateTime.Now;
            return (T)foundValue.value;
        }
    }
}
