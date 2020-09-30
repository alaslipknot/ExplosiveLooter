using HardBit.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAnimationEvent : MonoBehaviour
{

    PlayerComboAnimator pca;
    void Start()
    {
         pca = transform.parent.GetComponent<PlayerComboAnimator>();
    }

    public void SlashEvent (int i)
    {
        pca.PlaySlash(i);
    }

    public void SetDamage(int v)
    {
        pca.SetDamage(v);
    }

    public void SetCanMove(int v)
    {
        pca.SetCanMove(v);
    }

    public void SetInAttack(int v)
    {
        pca.SetInAttack(v);
    }
}
