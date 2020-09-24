/*
 * Refactor 
 * Damage =/= info
 * use PlayerDamage
 * use PlayerState
 * */

using UnityEngine;

namespace HardBit.Player {
    public class PlayerInfo : MonoBehaviour {

        public static PlayerInfo _instance;

        #region Exposed variables
        [SerializeField] private int _hp;
        [SerializeField] private int _maxHp;
        [SerializeField] private bool _isDead;
        #endregion




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

      
       

        public float GetHpRatio()
        {
            return (float)_hp / (float)_maxHp;
        }

        #region Dammage
        public bool IsDead { get => _isDead; set => _isDead = value; }
        #region Delegates events
        public delegate void OnDeathDelegate();
        public event OnDeathDelegate OnDeathEvent;
        #endregion
        public void TakeDamage(int damage, Vector3 enemyPosition)
        {
            _hp -= damage;
            if (_hp > 0)
            {
                PlayerSingleton._instance._playerHud.SetHpInfo(GetHpRatio(), _hp, _maxHp);
                PlayerSingleton._instance._playerMovement.SetRecoilDirectio(enemyPosition);
                PlayerSingleton._instance._playerDamage.OnDamageEvents();
            }
            else
            {
                PlayerSingleton._instance._playerHud.SetHpInfo(GetHpRatio(), _hp, _maxHp);
                PlayerSingleton._instance._playerMovement.SetRecoilDirectio(enemyPosition);
                RiseDeathEvent();
            }

        }

        public void RiseDeathEvent()
        {
            _isDead = true;

            if (OnDeathEvent != null)
            {
                OnDeathEvent();

            }
        }
        #endregion


    }
}
