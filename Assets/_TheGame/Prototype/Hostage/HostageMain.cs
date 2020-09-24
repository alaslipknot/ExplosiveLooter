
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System.Collections;

namespace HardBit.Hostages {
    public class HostageMain : MonoBehaviour {

        #region Exposed
        [Header("Events")]
        [SerializeField] private UnityEvent OnPickedUpEvent;
        [SerializeField] private UnityEvent OnPutDownEvent;
        [SerializeField] private UnityEvent OnGotSavedEvent;
        [Space(10)]
        [Header("Tweens")]
        [SerializeField] private float _tweenTime = 0.5f;
        [SerializeField] private Ease _tweenEase = Ease.OutBounce;
        [Header("Drop settings")]
        [SerializeField] private float _dropForceUp = 3;
        [SerializeField] private float _dropForceForward = 5;
        #endregion




        #region local variables
        private Transform _tempParent;
        private Transform _mainParent;
        private Vector3 _portalPosition;
        #endregion
        #region cached
        private Transform _transform;
        private Rigidbody _body;
        private BoxCollider _collider;
        #endregion
        private void Start()
        {
            GetCachedVariables();
        }

        void GetCachedVariables()
        {
            _transform = transform;

            _body = GetComponent<Rigidbody>();

            _collider = GetComponent<BoxCollider>();

        }
        public void Picked()
        {
            OnPickedUpEvent.Invoke();
            _collider.enabled = false;
            _body.isKinematic = true;
            _transform.parent = _tempParent;
            _transform.DOLocalMove(Vector3.zero, _tweenTime);
            _transform.DOLocalRotate(Vector3.zero, _tweenTime);
            _transform.DOScale(1, _tweenTime).SetEase(_tweenEase);
        }

        public void PutDown()
        {
            StartCoroutine(PutDownSequence());


        }


        IEnumerator PutDownSequence()
        {
            _transform.parent = null;
            // yield return new WaitForSeconds(1.15f);
            _body.velocity = Vector3.zero;
            _body.mass = 10;
            _body.isKinematic = false;
            _body.AddForce(_tempParent.forward * _dropForceForward, ForceMode.Impulse);
            _body.AddForce(_tempParent.up * _dropForceUp, ForceMode.Impulse);
            _collider.enabled = true;

            yield return new WaitForSeconds(0.5f);
            _body.mass = 500;
            _transform.DOScale(2, _tweenTime).SetEase(_tweenEase);
            OnPutDownEvent.Invoke();
            if (_mainParent != null)
            {
                _transform.parent = _mainParent;
            }

        }


        public void Saved()
        {
            StartCoroutine(SavedSequence());
        }

        IEnumerator SavedSequence()
        {
            _transform.parent = null;
            _transform.DOScale(2, _tweenTime).SetEase(_tweenEase);
            _portalPosition.y += 8; 
            _transform.DOMove(_portalPosition, 1.0f).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(1.0f);
            _transform.DOScale(0, _tweenTime).SetEase(Ease.InBack);
            if (_mainParent != null)
            {
                _transform.parent = _mainParent;
            }
            OnGotSavedEvent.Invoke();

        }

        public void SetNewTempParent(Transform t)
        {
            _tempParent = t;
        }
        public void SetMainParent(Transform t)
        {
            _mainParent = t;
        }

        public void SetPortalPosition(Vector3 p)
        {
            _portalPosition = p;
        }
    }
}