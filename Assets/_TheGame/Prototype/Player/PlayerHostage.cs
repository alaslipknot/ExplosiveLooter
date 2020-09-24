
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using HardBit.Specific.UserInput;
using HardBit.Hostages;

namespace HardBit.Player {


    public class PlayerHostage : MonoBehaviour {

        [SerializeField] private float _rayLength;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Transform _hostHolder;
        [SerializeField] private bool _isHoldingHostage;
        HostageMain _hostage;


        //Dumb MUST FIX
        public float _wait1 = 0.5f;
        public float _wait2 = 0.5f;
        public GameObject _weapon;
        void Start()
        {
        }

        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isHoldingHostage)
            {
                if (other.CompareTag("HostagePortal"))
                {
                    SaveHostage();
                    Debug.Log("saving hostage");
                }
            }
        }
        private void OnCollisionEnter(Collision other)
        {
            if (!_isHoldingHostage)
            {
                if (other.gameObject.CompareTag("Hostage"))
                {
                    InputHandler._instance.OnInteractEvent += pickupHostage;
                    Debug.Log("Subscribed from collision");
                    _hostage = other.gameObject.GetComponent<HostageMain>();
                }
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (!_isHoldingHostage)
            {
                if (other.gameObject.CompareTag("Hostage"))
                {

                    InputHandler._instance.OnInteractEvent -= pickupHostage;
                    _hostage = null;
                    Debug.Log("UnSubscribed from collision");
                }
            }
        }


        void SetPlayerStateToPickUp(bool isPickUp)
        {
            if (isPickUp)
            {
                PlayerSingleton._instance._playerAnimation.HandsUpAnimation();
                PlayerSingleton._instance._playerShooting.SetWeaponVisibility(false);
                PlayerSingleton._instance._playerAiming.SetCanAim(false);

            }
            else
            {
                PlayerSingleton._instance._playerAnimation.HandsDownAnimation();
                PlayerSingleton._instance._playerShooting.SetWeaponVisibility(true);
                PlayerSingleton._instance._playerAiming.SetCanAim(true);
            }
        }

        void pickupHostage()
        {


            if (_hostage == null) return;

            _isHoldingHostage = true;

            InputHandler._instance.OnInteractEvent -= pickupHostage;
            InputHandler._instance.OnInteractEvent += PutDownHostage;
            Debug.Log("picking up");

            SetPlayerStateToPickUp(true);

            _hostage.SetNewTempParent(_hostHolder);
            _hostage.Picked();

        }



        void PutDownHostage()
        {
            Debug.Log("puting down");
            _isHoldingHostage = false;
            InputHandler._instance.OnInteractEvent -= PutDownHostage;

            SetPlayerStateToPickUp(false);

            _hostage.PutDown();
        }

        void SaveHostage()
        {
            _isHoldingHostage = false;
            InputHandler._instance.OnInteractEvent -= PutDownHostage;
            SetPlayerStateToPickUp(false);
            _hostage.Saved();
        }
    }
}