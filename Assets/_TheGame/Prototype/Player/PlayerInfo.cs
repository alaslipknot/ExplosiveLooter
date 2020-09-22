/*
 * Refactor this class to use delegate Events
 * */

using UnityEngine;

namespace HardBit.Player {
    public class PlayerInfo : MonoBehaviour {

        [SerializeField] private int _hp;
        [SerializeField] private int _maxHp;
        [SerializeField] private bool _isDead;
        public delegate void OnDeathDelegate();
        public event OnDeathDelegate OnDeathEvent;
        //inner references
        PlayerDamage _playerDamage;
        PlayerHUD _playerHud;
        PlayerMovement _playerMove;
        PlayerAnimation _playerAnim;

        public bool IsDead { get => _isDead; set => _isDead = value; }

        private void Start()
        {
            CacheInnerVariables();
        }

        void CacheInnerVariables()
        {
            _playerDamage = GetComponent<PlayerDamage>();
            _playerHud = GetComponent<PlayerHUD>();
            _playerMove = GetComponent<PlayerMovement>();
            _playerAnim = GetComponent<PlayerAnimation>();
        }

        public float GetHpRatio()
        {
            return (float)_hp / (float)_maxHp;
        }

        public void TakeDamage(int damage, Vector3 enemyPosition)
        {
            _hp -= damage;
            if (_hp > 0)
            {
                _playerHud.SetHpInfo(GetHpRatio(), _hp, _maxHp);
                _playerMove.SetRecoilDirectio(enemyPosition);
                _playerDamage.OnDamageEvents();
            }
            else
            {
                _playerHud.SetHpInfo(GetHpRatio(), _hp, _maxHp);
                _playerMove.SetRecoilDirectio(enemyPosition);
                _playerDamage.OnDamageEvents();
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
    }
}
