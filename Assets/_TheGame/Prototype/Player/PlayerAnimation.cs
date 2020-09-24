using UnityEngine;

namespace HardBit.Player {
    public class PlayerAnimation : MonoBehaviour {

        #region Private fields
        private float _velX;
        private float _velY;
        #endregion

        #region Cached 
        //cached
        private Animator _anim;
        private PlayerInfo _playerInfo;
        private PlayerMovement _playerMove;
        private PlayerShooting _playerShoot;
        #endregion

        #region Properties
        #endregion


        private void Awake()
        {
            CacheInnerReference();
            EventSubscription();
        }


        void CacheInnerReference()
        {
            _anim = GetComponentInChildren<Animator>();
            _playerInfo = GetComponent<PlayerInfo>();
            _playerMove = GetComponent<PlayerMovement>();
            _playerShoot = GetComponent<PlayerShooting>();
        }

        void EventSubscription()
        {
            _playerShoot.OnShootingEvent += ShootAnimation;
            _playerInfo.OnDeathEvent += DeathAnimation;
        }

        private void Update()
        {
            SetMovingSpeed(_playerMove.MoveDirection, _playerMove.Direction);
        }



        void SetMovingSpeed(Vector2 joystick, int direction)
        {
            _velX = joystick.y * direction;
            _velY = joystick.x * direction;
            _anim.SetFloat("VelX", _velX);
            _anim.SetFloat("VelY", _velY);
        }

        void ShootAnimation()
        {
            _anim.SetTrigger("Shoot");
        }

        void DeathAnimation()
        {
            _anim.SetLayerWeight(1, 0);
            _anim.SetTrigger("Die");
        }

        public void HandsUpAnimation()
        {
            _anim.SetTrigger("HandsUp");
        }

        public void HandsDownAnimation()
        {
            _anim.SetTrigger("HandsDown");
        }
    }
}