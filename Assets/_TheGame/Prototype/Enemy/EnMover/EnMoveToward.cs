
using UnityEngine;

namespace HardBit.Enemies {
    public class EnMoveToward : EnMover {

        [SerializeField] private Transform _target;
        [SerializeField] private bool _lookForPlayer;
        [SerializeField] private bool _randomizeSpeed;
        [SerializeField] private Vector2 _randomSpeedRange = new Vector2(-2.0f, 2.0f);

        private void Awake()
        {
            if (_randomizeSpeed)
            {
                MaxSpeed += Random.Range(_randomSpeedRange.x, _randomSpeedRange.y);

            }
        }

        private void Start()
        {
            base.CacheReferences();
            if (_target == null && _lookForPlayer)
            {
                _target = EnemyTracker._instance.Player.transform;
            }
        }


        public override void Move()
        {
            if (!CanMove || _target == null)
            {
                return;
            }


            base.FaceTarget(_target);
            base.MoveForward();

        }

        private void FixedUpdate()
        {
            Move();
        }
    }
}