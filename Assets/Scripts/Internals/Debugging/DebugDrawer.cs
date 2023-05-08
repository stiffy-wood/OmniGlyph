using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OmniGlyph.Internals.Debugging {
    public class DebugDrawer : BaseMonoInternal {
        HashSet<Vector3> _gizmos = new HashSet<Vector3>();
        GameObject _debugSpherePrefab;
        GameObject _debugSectorPrefab;
        HashSet<GameObject> _debugObjects = new HashSet<GameObject>();
        public void StartDrawingGizmo(Vector3 gizmo) {
            lock (_gizmos) {
                _gizmos.Add(gizmo);
            }
        }
        public void StopDrawingGizmo(Vector3 gizmo) {
            lock (_gizmos) {
                _gizmos.Remove(gizmo);
            }
        }

        private GameObject SpawnObject(GameObject objectToSpawn, Vector3 position) {
            GameObject spawnedObject = Instantiate(objectToSpawn, position, Quaternion.identity);
            lock (_debugObjects) {
                _debugObjects.Add(spawnedObject);
            }
            return spawnedObject;
        }
        public void DestroyDebugObject(GameObject objectToDestroy) {
            lock (_debugObjects) {
                GameObject objectToDestroyObject = _debugObjects.Where(o => o == objectToDestroy).FirstOrDefault();
                if (objectToDestroyObject != null) {
                    _debugObjects.Remove(objectToDestroyObject);
                    Destroy(objectToDestroyObject);
                }
            }
        }
        public GameObject SpawnDebugSphere(Vector3 sphere) {
            return SpawnObject(_debugSpherePrefab, sphere);
        }
        public GameObject SpawnDebugSector(Vector3 sectorCenter, Vector3 sectorScale) {
            GameObject sectorToSpawn = _debugSectorPrefab;
            sectorToSpawn.transform.localScale = sectorScale;

            return SpawnObject(sectorToSpawn, sectorCenter);


        }

        void Start() {
        }
        public override void Init(InternalsManager manager) {
            base.Init(manager);
            _debugSpherePrefab = Resources.Load<GameObject>("Debug/DebugSphere");
            _debugSectorPrefab = Resources.Load<GameObject>("Debug/DebugSector");

        }
        private void OnDrawGizmos() {
            lock (_gizmos) {
                foreach (Vector3 gizmo in _gizmos) {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawSphere(gizmo, 1);
                }
            }
        }
    }
}
