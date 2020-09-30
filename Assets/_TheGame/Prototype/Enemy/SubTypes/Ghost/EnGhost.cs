
using HardBit.Specific.Gameplay;
using UnityEngine;
namespace HardBit.Enemies
{
    public class EnGhost : EnemyMain, IDamageable
    {
       void StandardDamage(int damage)
        {
            TakeDamageEvent.Invoke();

            Hp -= damage;
            if (Hp <= 0)
            {
                DeathEvent.Invoke();
            }
        }

        public void TakeDamage(int damage)
        {
            StandardDamage(damage);
            
        }

        public void TakeDamage(int damage, Vector3 direction)
        {
            StandardDamage(damage);
            Body.AddForce(direction, ForceMode.Impulse);
        }
    }
}
