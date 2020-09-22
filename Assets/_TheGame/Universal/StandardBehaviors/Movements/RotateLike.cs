using UnityEngine;

namespace HardBit.Universal.StandardBehaviors.Movements
{
    public class RotateLike : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _smoothness = 0.1f;
        [SerializeField] private Vector3Int _followAt = new Vector3Int(1, 1, 1);

        private void LateUpdate()
        {
            if (_target == null) return;

            var rot = transform.eulerAngles;
            rot.x = _followAt.x > 0 ? _target.eulerAngles.x : transform.eulerAngles.x;
            rot.y = _followAt.y > 0 ? _target.eulerAngles.y : transform.eulerAngles.y;
            rot.z = _followAt.z > 0 ? _target.eulerAngles.z : transform.eulerAngles.z;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, rot, _smoothness);
        }
    }
}