using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HardBit.Hostages {
    public class HostageSpawnPositions : MonoBehaviour {


        public static HostageSpawnPositions _instance;

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