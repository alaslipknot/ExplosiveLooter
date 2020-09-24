using HardBit.Player;
using UnityEngine;
using HardBit.Specific.Gameplay;

namespace HardBit.Specific.Weapons {


    public class Weapon : MonoBehaviour {
        [SerializeField] private WeaponTemplate _weaponTemplate;
        [SerializeField] private Transform _gunPort;
        [SerializeField] private Vector3 _gunPortOffset = new Vector3(0, 0, 0.5f);
        [SerializeField] private float _impactOffset = 0.5f;
        [SerializeField] private GameObject _weaponGraphics;
        private float _coolDownCounter;
        private bool _canShoot;

        public void Shoot(PlayerShooting shooter)
        {
            if (!_canShoot) return;


            var pos = _gunPort.position;
            SendProjectile(_gunPort, _gunPortOffset);
            ShowFlash(_gunPort, _gunPortOffset);
            shooter.RiseShootingEvent();
            _coolDownCounter = _weaponTemplate.Cooldown;
        }


        void ShowFlash(Transform parent, Vector3 offset)
        {
            GameObject go = Instantiate(_weaponTemplate.BulletTemplate._flash);
            go.transform.forward = parent.forward;
            go.transform.position = parent.position + offset;
        }

        void SendProjectile(Transform parent, Vector3 offset)
        {
            GameObject go = Instantiate(_weaponTemplate.BulletTemplate._projectile);
            go.SetActive(false);
            go.transform.forward = parent.forward;
            go.transform.position = parent.position + offset;
            Projectile p = go.GetComponent<Projectile>();
            p.OnCollidedEvent += DealDamage;
            p.SetSpeed(_weaponTemplate.BulletTemplate._speed);
            go.SetActive(true);
        }

        void PlaceImpact(Vector3 position)
        {
            GameObject go = Instantiate(_weaponTemplate.BulletTemplate._impact);
            go.transform.eulerAngles = transform.eulerAngles + new Vector3(0, 180, 0);
            position += go.transform.forward * _impactOffset;
            go.transform.position = position;
        }

        void SetParent(Transform child, Transform parent, bool doRotation, bool doPosition)
        {
            child.parent = parent;
            if (doRotation)
                child.localEulerAngles = Vector3.zero;
            if (doPosition)
                child.localPosition = Vector3.zero;
        }

        public void ShootReset()
        {
            _coolDownCounter = 0;
        }

        private void Update()
        {

            _coolDownCounter -= Time.deltaTime;
            _canShoot = _coolDownCounter <= 0;


        }



        public void DealDamage(Collision coll)
        {
            IDamageable damageable = coll.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Debug.Log("Dealing " + _weaponTemplate.Damage + " Damage" + " @" + coll.gameObject.name);
                damageable.TakeDamage(_weaponTemplate.Damage);
                PlaceImpact(coll.contacts[0].point);
            }
        }

        public void SetWeaponVisiblility(bool b)
        {
            _weaponGraphics.SetActive(b);
        }
    }
}