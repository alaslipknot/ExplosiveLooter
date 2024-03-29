﻿/*
 * Singleton object that contains the possible positions enemies can spawn from in the scene
 */
using UnityEngine;

namespace HardBit.Enemies {

    public class EnemySpawnPositions : MonoBehaviour {

        public static EnemySpawnPositions _instance;

        public Vector3[] _spawnList;


        void InitSpawnList()
        {
            _spawnList = new Vector3[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                _spawnList[i] = transform.GetChild(i).position;
            }
            //Destroy all children
        }

        private void OnEnable()
        {


            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this);
                return;
            }

            if (_spawnList.Length == 0)
            {
                InitSpawnList();
            }
        }

    }
}