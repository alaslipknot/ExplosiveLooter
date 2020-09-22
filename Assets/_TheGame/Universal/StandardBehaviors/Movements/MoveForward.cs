
using UnityEngine;
namespace HardBit.Universal.StandardBehaviors.Movements
{
    public class MoveForward : MonoBehaviour
    {

        [SerializeField] private float _speed;
        private Transform _transform;
        private void Awake()
        {
            _transform = this.transform;
        }

        void Update()
        {
            _transform.Translate(_transform.forward * _speed * Time.deltaTime, Space.World);
        }
    }
}