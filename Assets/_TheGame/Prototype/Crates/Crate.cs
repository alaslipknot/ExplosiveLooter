using HardBit.Specific.Gameplay;
using UnityEngine;
using UnityEngine.Events;

public class Crate : MonoBehaviour, IDamageable
{

    [SerializeField] private int _hp = 5;
    [SerializeField] private UnityEvent _damageEffectEvent;
    public void DamageEffect()
    {
        _damageEffectEvent.Invoke();
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        DamageEffect();
        if (_hp <= 0)
        {
            DestroyObject();
        }
    }

    void DestroyObject()
    {
        gameObject.SetActive(false);
    }


}
