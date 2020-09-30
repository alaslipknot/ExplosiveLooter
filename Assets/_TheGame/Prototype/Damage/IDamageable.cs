using UnityEngine;

namespace HardBit.Specific.Gameplay
{
    public interface IDamageable
    {
        void TakeDamage(int damage);

        void TakeDamage(int damage,Vector3 direction);
    }
}

