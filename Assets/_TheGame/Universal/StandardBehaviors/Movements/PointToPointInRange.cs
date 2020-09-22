using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HardBit.Universal.Math;

namespace HardBit.Universal.StandardBehaviors.Movements {
    public class PointToPointInRange : MonoBehaviour {

        [SerializeField] private float _speed;
        [SerializeField] private Vector3 _minRange;
        [SerializeField] private Vector3 _maxRange;
        [SerializeField] private bool _lookAt;
        [SerializeField] private bool _ignoreY;

        private Vector3 _targetPos;
        private Transform _transform;
        void Start()
        {
            _transform = this.transform;
            SetTargetPos();
        }

        void Update()
        {
            if (_lookAt)
            {
                MoveForwardAndLookAt();
            }
            else
            {
                MoveDirectionally();
            }

            UpdateWhenReached();
        }

        void MoveDirectionally()
        {

        }

        void MoveForwardAndLookAt()
        {
            _transform.Translate(_transform.forward * _speed * Time.deltaTime, Space.World);
            _transform.LookAt(_targetPos, _transform.up);
        }


        void UpdateWhenReached()
        {
            if (Vector3.Distance(_transform.position, _targetPos) <= 0.5f)
            {
                SetTargetPos();
            }
        }

        void SetTargetPos()
        {
            _targetPos = MathHelp.Vec3InRange(_minRange, _maxRange);
            if (_ignoreY)
            {
                _targetPos.y = _transform.position.y;
            }
        }
    }
}