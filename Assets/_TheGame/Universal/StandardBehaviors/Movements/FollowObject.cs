using UnityEngine;

namespace HardBit.Universal.StandardBehaviors.Movements {

    public class FollowObject : MonoBehaviour {

        [SerializeField] private Transform _target;
        [Range(0.0f, 0.5f)]
        [SerializeField] private float _smoothness = 0.1f;
        [SerializeField] private Vector3Int _followAt = new Vector3Int(1, 1, 1);
        [SerializeField] private bool _useFixedUpdate;


        private void Awake()
        {
            Snap();
        }

        private void FixedUpdate()
        {
            if (_useFixedUpdate)
            {
                if (_target == null) return;

                Smooth();
            }

        }

        private void Update()
        {
            if (!_useFixedUpdate)
            {
                if (_target == null) return;

                Smooth();
            }
        }

        private void Smooth()
        {
            var pos = GetTargetPos();
            transform.position = Vector3.Lerp(transform.position, pos, _smoothness);
        }

        private void Snap()
        {
            var pos = GetTargetPos();
            transform.position = pos;
        }

        private Vector3 GetTargetPos()
        {
            var pos = transform.position;
            pos.x = _followAt.x > 0 ? _target.position.x : transform.position.x;
            pos.y = _followAt.y > 0 ? _target.position.y : transform.position.y;
            pos.z = _followAt.z > 0 ? _target.position.z : transform.position.z;
            return pos;
        }
    }
}


