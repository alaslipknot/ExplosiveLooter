using UnityEngine;
using UnityEngine.Events;


namespace HardBit.Player {
    public class PlayerDamage : MonoBehaviour {
        [SerializeField] private UnityEvent _takeDamageEvents;

        public void OnDamageEvents()
        {

            _takeDamageEvents.Invoke();
        }


    }
}