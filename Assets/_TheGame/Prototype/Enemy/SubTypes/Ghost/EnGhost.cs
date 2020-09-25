
using HardBit.Specific.Gameplay;
using UnityEngine;
namespace HardBit.Enemies
{
    public class EnGhost : EnemyMain, IDamageable
    {
       

        public void TakeDamage(int damage)
        {
            TakeDamageEvent.Invoke();

            Hp--;
            if (Hp <= 0)
            {
                DeathEvent.Invoke();
            }
        }
    }
}
