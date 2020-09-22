
using UnityEngine;

namespace HardBit.Enemies {
    public class EnMover : MonoBehaviour {
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _velocityForce;
        [SerializeField] private bool _canMove;
        private Transform _transform;
        private Rigidbody _body;

        public bool CanMove { get => _canMove; set => _canMove = value; }
        public float MaxSpeed { get => _maxSpeed; set => _maxSpeed = value; }
        public float VelocityForce { get => _velocityForce; set => _velocityForce = value; }
        public Transform MyTransform { get => _transform; set => _transform = value; }
        public Rigidbody Body { get => _body; set => _body = value; }

        public void CacheReferences()
        {
            _transform = this.transform;
            _body = GetComponent<Rigidbody>();
        }

        public virtual void Move()
        {

        }

        public virtual void StopMoving()
        {
            _canMove = false;
        }

        public void FaceTarget(Vector3 position)
        {
            _transform.LookAt(position, _transform.up);
            var euler = _transform.eulerAngles;
            euler.x = 0; euler.z = 0;
            _transform.eulerAngles = euler;

        }

        public void FaceTarget(Transform _target)
        {
            _transform.LookAt(_target, _transform.up);
            var euler = _transform.eulerAngles;
            euler.x = 0; euler.z = 0;
            _transform.eulerAngles = euler;

        }

        public void MoveForward()
        {
            if (_body.isKinematic)
            {
                _transform.Translate(_transform.forward * _maxSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                _body.AddForce(_transform.forward * VelocityForce);
                _body.velocity = Vector3.ClampMagnitude(_body.velocity, MaxSpeed);
            }

        }

    }
}