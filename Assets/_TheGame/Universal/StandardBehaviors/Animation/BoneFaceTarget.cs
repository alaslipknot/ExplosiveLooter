using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HardBit.Universal.Animation {
    public class BoneFaceTarget : MonoBehaviour {
        [Header("Transforms")]
        [SerializeField] private Transform _origin;
        [SerializeField] private Transform _bone;
        [SerializeField] private Transform _lookAtTarget;
        [Header("Settings")]
        [SerializeField] private float _weight = 1.0f;
        [SerializeField] private Vector3 _lookAtOffset = new Vector3(0, -45, -110);
        [Header("Debug")]
        [SerializeField] private bool _doDrawGizmos;

        public Transform LookAtTarget { get => _lookAtTarget; set => _lookAtTarget = value; }
        public float Weight { get => _weight; set => _weight = value; }

        void Start()
        {
            if (_origin == null)
            {
                _origin = this.transform;
            }
        }


        void LateUpdate()
        {
            LookAt();
        }


        void LookAt()
        {

            if (_bone == null || _lookAtTarget == null || _origin == null) return;

            Quaternion targetRot = Quaternion.LookRotation(_lookAtTarget.position - _origin.position);
            targetRot = targetRot * Quaternion.Euler(_lookAtOffset);
            _bone.rotation = Quaternion.Lerp(_bone.rotation, targetRot, _weight);

        }

        

        private void OnDrawGizmos()
        {
            if (!_doDrawGizmos) return;
            Debug.DrawLine(_origin.position, _lookAtTarget.position, Color.red);
        }
    }
}
