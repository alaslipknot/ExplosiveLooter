using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HardBit.Specific.Gameplay;

public class MeleeDamageCollider : MonoBehaviour {
    public int DamagePower;
    public int RecoilPower;
    private void OnTriggerEnter(Collider other)
    {
        DealDamage(other);
    }

    public void DealDamage(Collider coll)
    {
        IDamageable damageable = coll.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // Debug.Log("Dealing " + _weaponTemplate.Damage + " Damage" + " @" + coll.gameObject.name);
            Vector3 dir = transform.parent.transform.forward * RecoilPower; //(coll.transform.position - transform.parent.transform.position) * RecoilPower;
            damageable.TakeDamage(DamagePower, dir);
        }
    }
}
