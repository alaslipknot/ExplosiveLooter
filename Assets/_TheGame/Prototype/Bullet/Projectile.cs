//Move a Projectile forward
//Has a delegate/events that fires up OnCollisionEnter
//Event get called from subscribed objects
//Disable the object OnCollisionEnter

using UnityEngine;

namespace HardBit.Specific.Weapons
{
    public class Projectile : MonoBehaviour
    {
        private float _speed;
        private Transform _transform;
        public delegate void OnCollisionDelegate(Collision coll);
        public event OnCollisionDelegate OnCollidedEvent;

        private void Awake()
        {
            _transform = this.transform;
        }

        // Update is called once per frame
        void Update()
        {
            _transform.Translate(_transform.forward * _speed * Time.deltaTime, Space.World);
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collision from Main Function");
            if (OnCollidedEvent != null)
            {
                OnCollidedEvent(collision);
            }
            gameObject.SetActive(false);
        }
    }
}