using UnityEngine;
using UnityEngine.Events;

namespace HardBit.Universal.StandardBehaviors.Events
{

    public class CollisionEvents : MonoBehaviour
    {

        [SerializeField] private UnityEvent _OnCollisionEnter;
        [SerializeField] private UnityEvent _OnCollisionExit;
        [SerializeField] private UnityEvent _OnCollisionStay;


        private void OnCollisionEnter(Collision collision)
        {
            _OnCollisionEnter.Invoke();
        }

        private void OnCollisionExit(Collision collision)
        {
            _OnCollisionExit.Invoke();
        }

        private void OnCollisionStay(Collision collision)
        {
            _OnCollisionStay.Invoke();
        }
    }
}
