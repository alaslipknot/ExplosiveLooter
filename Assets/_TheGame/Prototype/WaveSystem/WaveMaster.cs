using HardBit.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HardBit.WaveSystem {
    public class WaveMaster : MonoBehaviour {

        [SerializeField] private WaveGenerator _currentWave;
        private EnemyTracker _enTracker;


        void Start()
        {
            GetSingletons();
            GetReferences();
        }


        void GetSingletons()
        {

            _enTracker = EnemyTracker._instance;
        }

        void GetReferences()
        {
            _currentWave = GetComponentInChildren<WaveGenerator>();
        }

        void CheckWaveStatus()
        {
            if (_currentWave.NoMoreSplits())
            {

                if (_enTracker.IsAllDead())
                {
                    Debug.Log("Wave Finished!!");
                }
                else
                {
                    Debug.Log("Some enemies still alive");
                }

            }
            else
            {
                Debug.Log("still more splits");
            }
        }

        public void OnEnemyDeath()
        {
            CheckWaveStatus();
        }

    }
}