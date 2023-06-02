using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OmniGlyph.Configs;
using OmniGlyph.Control;
using UnityEngine;

namespace OmniGlyph.Internals.Debugging {
    public class InternalDebugger : MonoBehaviour {
        private List<GameObject> _watchedPositions = new List<GameObject>();
        private GameObject _visualizedObj = null;
        private Color defaultDebugColor = Color.cyan;

        private GameObject CreateDebugObject(DebugObjectProperties properties) {
            GameObject debugObject = GameObject.CreatePrimitive(properties.Type);
            debugObject.GetComponent<MeshRenderer>().material.color = properties.Color;
            debugObject.transform.localScale = properties.Size;
            return debugObject;
        }
        private bool IsDebug() {
            return InternalConfig.IsDebug;
        }
        public void Log(object message) {
            if (!IsDebug())
                return;
            Debug.Log(message);
        }
        public void LogWarning(object message) {
            Debug.LogWarning(message);
        }
        public void LogError(object message) {
            Debug.LogError(message);
        }

        public void Watch(Vector3 watchingPos, DebugObjectProperties properties = null) {
            if (!IsDebug())
                return;
            if (properties == null) {
                properties = new DebugObjectProperties(Vector3.one, defaultDebugColor);
            }
            GameObject debugObject = CreateDebugObject(properties);
            debugObject.transform.position = watchingPos;
            debugObject.transform.SetParent(transform, true);
            _watchedPositions.Add(debugObject);

        }
        public void Watch(DebugObjectProperties properties = null) {
            if (!IsDebug())
                return;

            if (properties == null)
                properties = new DebugObjectProperties(Vector3.one, defaultDebugColor);

            _visualizedObj = CreateDebugObject(properties);
            _visualizedObj.transform.SetParent(transform, true);
        }
        public void StopWatching(Vector3 watchingPos, DebugObjectProperties properties = null) {
            if (!IsDebug())
                return;
            for (int i = 0; i < _watchedPositions.Count; i++) {
                if (_watchedPositions[i].transform.position == watchingPos) {
                    Destroy(_watchedPositions[i]);
                    _watchedPositions.RemoveAt(i);
                    return;
                }
            }
        }
        public void StopWatching() {
            if (!IsDebug())
                return;

            Destroy(_visualizedObj);
            _visualizedObj = null;
        }
        public void ThrowCriticalError(object message) {
            LogError(message);
            if (!IsDebug())
                return;
# if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        public void StopWatchingAll() {
            StopWatching();
            foreach (GameObject watchedPos in _watchedPositions) {
                Destroy(watchedPos);
            }
            _watchedPositions.Clear();
        }
        private void Update() {
            if (!IsDebug())
                return;
            if (_visualizedObj != null) {
                _visualizedObj.transform.position = transform.position;
            }
        }
    }
}
