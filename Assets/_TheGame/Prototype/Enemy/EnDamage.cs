using HardBit.Specific.Gameplay;
using UnityEngine;
using UnityEngine.Events;

namespace HardBit.Enemies
{
    public class EnDamage : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _hp = 5;
        [SerializeField] private UnityEvent _damageEffectEvent;
        [SerializeField] private UnityEvent _deathEvent;


        public void DamageEffect()
        {
            _damageEffectEvent.Invoke();
        }

        public void TakeDamage(int damage)
        {
            _hp--;
            DamageEffect();
            if (_hp < 0)
            {
                _deathEvent.Invoke();
            }
        }


    }
}