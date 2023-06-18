using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Cam {
    [CreateAssetMenu(fileName = "NewCameraContext", menuName = "Contexts/CameraContext")]
    public class CameraContext : ScriptableObject {
        [SerializeField]
        string _focusObjectTag;
        [SerializeField]
        Vector3 _offset;
        [SerializeField]
        float _lag;
        [SerializeField]
        float _changeFocusLag;

        public string FocusObjectTag => _focusObjectTag;
        public Vector3 Offset => _offset;
        public float Lag => _lag;
        public float ChangeFocusLag => _changeFocusLag;

    }
}
