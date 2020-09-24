using HardBit.Specific.Weapons;
using UnityEngine;

namespace HardBit.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        private Transform _transform;
        public delegate void OnShootingDelegate();
        public event OnShootingDelegate OnShootingEvent;

        private void Awake()
        {
            _transform = this.transform;
        }
        public void Shoot()
        {
            _weapon.Shoot(this);
          
            
        }


        public void RiseShootingEvent()
        {
            if (OnShootingEvent != null)
            {
                OnShootingEvent();

            }
        }

        public void ShootReset()
        {
            _weapon.ShootReset();
        }

        public void SetWeaponVisibility(bool b)
        {
            _weapon.SetWeaponVisiblility(b);
        }
    }
}