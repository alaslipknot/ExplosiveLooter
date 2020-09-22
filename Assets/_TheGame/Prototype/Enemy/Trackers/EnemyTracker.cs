/*
 * Singleton
 * EnemyMain add and remove itself automatically to this Class
 * Used by PlayerAiming to easily find the nearest enemy
*/

using HardBit.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HardBit.Enemies {

    public class EnemyTracker : MonoBehaviour {

        public static EnemyTracker _instance;
        [SerializeField] private List<EnemyMain> _listOfEnemies;
        [SerializeField] private PlayerInfo _player;
        [SerializeField] private bool _allAreDead;
        public List<EnemyMain> ListOfEnemies { get => _listOfEnemies; set => _listOfEnemies = value; }
        public PlayerInfo Player { get => _player; set => _player = value; }
        public bool AllAreDead { get => _allAreDead; set => _allAreDead = value; }

        private void OnEnable()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }

            if (_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
            }
        }

        public void CheckAllDead()
        {

            for (int i = 0; i < _listOfEnemies.Count; i++)
            {

            }
        }


    }
}