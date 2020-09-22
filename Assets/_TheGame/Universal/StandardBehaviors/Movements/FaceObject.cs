using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HardBit.Universal.StandardBehaviors.Movements
{
    public class FaceObject : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private Transform _transform;

        public Transform Target { get => _target; set => _target = value; }

        void Awake()
        {
            _transform = this.transform;
        }

        void Update()
        {
            if (_target != null)
            {
                _transform.LookAt(_target,Vector3.up);
            }
        }
    }
}