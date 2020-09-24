
using UnityEngine;

namespace HardBit.Player {
    public class PlayerSingleton : MonoBehaviour {


        #region Singleton
        public static PlayerSingleton _instance;
        private void OnEnable()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        #region public cached references
        [HideInInspector] public PlayerInfo _playerInfo;
        [HideInInspector] public PlayerHUD _playerHud;
        [HideInInspector] public PlayerDamage _playerDamage;
        [HideInInspector] public PlayerMovement _playerMovement;
        [HideInInspector] public PlayerShooting _playerShooting;
        [HideInInspector] public PlayerAiming _playerAiming;
        [HideInInspector] public PlayerAnimation _playerAnimation;
        [HideInInspector] public PlayerHostage _playerHostage;
        #endregion


        void Start()
        {
            GetCachedReference();
        }

        void GetCachedReference()
        {
            _playerInfo = GetComponent<PlayerInfo>();
            _playerHud = GetComponent<PlayerHUD>();
            _playerDamage = GetComponent<PlayerDamage>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerShooting = GetComponent<PlayerShooting>();
            _playerAiming = GetComponent<PlayerAiming>();
            _playerAnimation = GetComponent<PlayerAnimation>();
            _playerHostage = GetComponent<PlayerHostage>();
        }
    }
}
