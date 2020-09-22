using UnityEngine;

namespace HardBit.Enemies {
    public class EnMoveRoam : EnMover {
        [SerializeField] private Vector3 _minRange;
        [SerializeField] private Vector3 _maxRange;
        [SerializeField] private bool _lookAt;
        [SerializeField] private bool _ignoreY;

        private Vector3 _targetPos;
        void Start()
        {
            base.CacheReferences();
            SetTargetPos();
        }

        void FixedUpdate()
        {
            if (_lookAt)
            {

                base.FaceTarget(_targetPos);

            }

            base.MoveForward();
            UpdateWhenReached();
        }





        void UpdateWhenReached()
        {
            if (Vector3.Distance(base.MyTransform.position, _targetPos) <= 1f)
            {
                SetTargetPos();
            }
        }

        void SetTargetPos()
        {
            _targetPos = Universal.Math.MathHelp.Vec3InRange(_minRange, _maxRange);
            if (_ignoreY)
            {
                _targetPos.y = base.MyTransform.position.y;
            }
        }
    }
}
