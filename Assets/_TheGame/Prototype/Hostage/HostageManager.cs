using UnityEngine;
using HardBit.Universal.Math;
using System.Collections.Generic;
using HardBit.Universal.StandardBehaviors.Movements;

namespace HardBit.Hostages {
    public class HostageManager : MonoBehaviour {

        [Header("Gameplay settings")]
        [SerializeField] private int _totalHostage;
        [Header("Class settings")]
        [SerializeField] private GameObject _prefabHostage;
        [SerializeField] private List<HostageMain> _listHostage = new List<HostageMain>();
        [SerializeField] private HostageSpawnPositions _spawnPositions;
        [SerializeField] private int _spawnPosIndex;
        void Start()
        {
            _spawnPositions = HostageSpawnPositions._instance;
            SpawnHostages();
            FindPlayerForHostages();
        }


        void SpawnHostages()
        {
            for (int i = 0; i < _totalHostage; i++)
            {
                CreateHostage();
            }
        }

        void FindPlayerForHostages()
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            for (int i = 0; i < _listHostage.Count; i++)
            {
                _listHostage[i].GetComponent<FaceObject>().Target = player;
            }
        }

        void CreateHostage()
        {
            GameObject go = Instantiate(_prefabHostage) as GameObject;
            HostageMain host = go.GetComponent<HostageMain>();
            host.transform.position = _spawnPositions._spawnList[_spawnPosIndex];
            host.transform.parent = transform;
            _listHostage.Add(host);
            _spawnPosIndex++;
        }



    }
}