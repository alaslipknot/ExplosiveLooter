using UnityEngine;
using DG.Tweening;
using HardBit.Specific.UserInput;
using System.Collections;

namespace HardBit.Player {
    public class PlayerMovement : MonoBehaviour {
        #region Private variables


        #region Movement & inputs
        [Header("Settings")]
        [SerializeField] private float _maxVelocity = 10;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private bool _canMove = true;
        [SerializeField] private bool _isRecoiling;
        [SerializeField] private float _recoilDelay = 0.5f;
        private float _currentSpeed;
        [SerializeField] private bool _faceDirection = true;
        [SerializeField] private int _direction = 1;
        private Vector2 _moveDirection;
        #endregion

        #region Taking damage & effects
        [SerializeField] private float _recoilForce = 5;
        [SerializeField] private Vector3 _recoilDirection;
        #endregion
        [Space(10)]

        [Header("Required References")]

        [SerializeField] private InputHandler _inputHandler;
        #endregion

        #region Properties
        public float CurrentSpeed { get => _currentSpeed; set => _currentSpeed = value; }
        public Vector2 MoveDirection { get => _moveDirection; set => _moveDirection = value; }
        public int Direction { get => _direction; set => _direction = value; }
        #endregion

        #region Cached variables
        private Transform _transform;
        private Rigidbody _body;
        private Transform _camera;
        private PlayerInfo _playerInfo;
        #endregion


        private void Awake()
        {

            CacheInnerReference();
            EventSubscription();

        }


        void CacheInnerReference()
        {
            _body = GetComponent<Rigidbody>();
            _camera = Camera.main.transform;
            _transform = this.transform;
            _playerInfo = GetComponent<PlayerInfo>();
        }

        void EventSubscription()
        {
            _playerInfo.OnDeathEvent += OnDeathDo;
        }

        bool isFlipping = false;



        public void FlipDirection()
        {
            if (isFlipping) return;

            Direction = -Direction;

            if (_faceDirection)
            {
                return;
            }


            isFlipping = true;
            var rot = _transform.eulerAngles;
            rot.y += 180;
            _transform.DORotate(rot, 0.15f).SetEase(Ease.OutSine).OnComplete(FlippingComplete);


        }

        private void FlippingComplete()
        {
            isFlipping = false;
        }

        private void FixedUpdate()
        {
            if (!_canMove) return;

            if (_isRecoiling) return;

            _moveDirection = _inputHandler.DirectionLeft;
            MoveDirectionally();

        }

        private void MoveDirectionally()
        {
            if (Mathf.Abs(_inputHandler.DirectionLeft.magnitude) > 0)
            {

                _currentSpeed = _moveSpeed * _inputHandler.DirectionLeft.magnitude;

                // _transform.Translate(_currentSpeed * MoveVector() * _direction * Time.deltaTime);
                _body.AddForce(_currentSpeed * MoveVector(), ForceMode.Impulse);
                _body.velocity = Vector3.ClampMagnitude(_body.velocity, _maxVelocity);
            }

        }

        private void MoveForward()
        {
            _currentSpeed = _moveSpeed * _inputHandler.DirectionLeft.magnitude;
            _transform.Translate(_currentSpeed * transform.forward * _direction * Time.deltaTime, Space.World);
        }

        private void FaceJoystick()
        {
            if (Mathf.Abs(_inputHandler.DirectionRight.magnitude) > 0)
            {
                var yRotation = Mathf.Atan2(_inputHandler.DirectionRight.x, _inputHandler.DirectionRight.y) * Mathf.Rad2Deg;
                var euler = _transform.eulerAngles;
                euler.y = yRotation + _camera.eulerAngles.y * _direction;
                _transform.eulerAngles = euler;
            }
        }

        public Vector3 MoveVector()
        {
            //return new Vector3(_inputHandler.DirectionLeft.x, 0, _inputHandler.DirectionLeft.y);
            var camForward = _camera.forward;
            var camRight = _camera.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward = camForward.normalized;
            camRight = camRight.normalized;
            return (camForward * _inputHandler.DirectionLeft.y) + (camRight * _inputHandler.DirectionLeft.x);

        }


        public void Recoil()
        {
            StopCoroutine("RecoilSequence");
            StartCoroutine("RecoilSequence");
        }

        IEnumerator RecoilSequence()
        {
            _recoilDirection.y = 0;
            _body.AddForce(_recoilDirection * _recoilForce, ForceMode.Impulse);
            _isRecoiling = true;
            yield return new WaitForSeconds(_recoilDelay);
            _isRecoiling = false;
        }

        public void SetRecoilDirectio(Vector3 recoilDir)
        {
            _recoilDirection = _transform.position - recoilDir;
        }

        void OnDeathDo()
        {
            _canMove = false;
        }
    }

}