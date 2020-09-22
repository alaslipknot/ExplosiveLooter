
using HardBit.Specific.Gameplay;
using UnityEngine;
namespace HardBit.Enemies
{
    public class EnGhost : EnemyMain, IDamageable
    {
       

        public void TakeDamage(int damage)
        {
            TakeDamageEvent.Invoke();
            print("taking cunt");
            Hp--;
            if (Hp <= 0)
            {
                print("death");
                DeathEvent.Invoke();
            }
        }
    }
}
