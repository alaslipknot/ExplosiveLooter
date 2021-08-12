using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HardBit.Enemies {
    public class EnDealDamage : MonoBehaviour {
        [SerializeField] private int _harmPower;


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                EnemyTracker._instance.Player.TakeDamage(_harmPower, transform.position);
            }
        }
    }
}