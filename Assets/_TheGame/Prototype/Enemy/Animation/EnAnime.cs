using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnAnime : MonoBehaviour
{
  [SerializeField]  private Animator _animator;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }


    public void AttackTrigger()
    {

    }

    public void DamageTrigger()
    {
        _animator.SetTrigger("TakeDamage");
    }
    public void DieTrigger()
    {
        _animator.SetTrigger("Die");
    }

}
