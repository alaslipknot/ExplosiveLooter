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
        public List<EnemyMain> ListOfEnemies { get => _listOfEnemies; set => _listOfEnemies = value; }
        public PlayerInfo Player { get => _player; set => _player = value; }

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

        public bool IsAllDead()
        {
            return _listOfEnemies.Count == 0;


        }


    }
}