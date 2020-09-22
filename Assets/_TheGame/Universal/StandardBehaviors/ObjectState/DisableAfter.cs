
using System.Collections;
using UnityEngine;

namespace HardBit.Universal.StandardBehaviors.ObjectState {
    public class DisableAfter : MonoBehaviour {

        [SerializeField] private float _time;
        [SerializeField] private bool _andDestroy;
        [SerializeField] private bool _onStart;

        private void Start()
        {
            if (_onStart)
            {
                Disable();
            }
        }

        public void Disable()
        {
            StartCoroutine(sequence());
        }

        IEnumerator sequence()
        {
            yield return new WaitForSeconds(_time);
            if (_andDestroy)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}