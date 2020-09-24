using UnityEngine;
using System.Collections.Generic;
using HardBit.Universal.StandardBehaviors.Movements;

namespace HardBit.Hostages {
    public class HostageManager : MonoBehaviour {

        [Header("Gameplay settings")]
        [SerializeField] private int _totalHostage;
        [Header("Class settings")]
        [SerializeField] private GameObject _prefabHostage;
        [SerializeField] private Transform _portal;
        private List<HostageMain> _listHostage = new List<HostageMain>();
        private HostageSpawnPositions _spawnPositions;
        private int _spawnPosIndex;
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
            host.SetMainParent(transform);
            host.SetPortalPosition(_portal.position);
            _listHostage.Add(host);
            _spawnPosIndex++;
        }



    }
}