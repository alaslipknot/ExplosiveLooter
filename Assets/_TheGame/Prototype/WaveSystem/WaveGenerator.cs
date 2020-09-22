using HardBit.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HardBit.WaveSystem {
    public class WaveGenerator : MonoBehaviour {

        [SerializeField] private WaveSpec _wave;
        [SerializeField] private List<WaveSplitTracker> _splitList = new List<WaveSplitTracker>();
        [Range(0.1f, 1.0f)]
        [SerializeField] private float _spawnDelayPerEnemy = 0.25f;
        [Range(0.1f, 3.0f)]
        [SerializeField] private float _splitTrackDelay = 2.0f;

        EnemySpawnPositions _spawnPositions;

        private void Start()
        {
            _spawnPositions = EnemySpawnPositions._instance;

            LaunchWave();
        }

        void LaunchWave()
        {
            for (int i = 0; i < _wave.enemies.Length; i++)
            {
                PrepareEnemies(i);
            }
        }


        void PrepareEnemies(int index)
        {
            WaveEnemyData enemyData = _wave.enemies[index];

            if (enemyData.splitWave)
            {
                StartCoroutine(SpawnSplitOneByOne(MakeSplitTracker(index, enemyData), enemyData, _spawnDelayPerEnemy));
            }
            else
            {
                StartCoroutine(SpawnOneByOne(enemyData, _spawnDelayPerEnemy));
            }
        }


        WaveSplitTracker MakeSplitTracker(int index, WaveEnemyData enemyData)
        {
            WaveSplitTracker wst = new WaveSplitTracker();
            wst._enemyData = enemyData;
            wst._splitNum = enemyData.splitNum;
            wst._totalRequired = enemyData.totalInWave;
            _splitList.Add(wst);
            return wst;
        }

        IEnumerator SpawnSplitOneByOne(WaveSplitTracker wst, WaveEnemyData enemyData, float delay)
        {
            for (int i = 0; i < wst._splitNum; i++)
            {
                Vector3 pos = _spawnPositions._spawnList[Random.Range(0, _spawnPositions._spawnList.Length)];
                pos.x += Random.Range(-2.0f, 2.0f);
                pos.z += Random.Range(-2.0f, 2.0f);
                EnemyMain en = SpawnEnemy(enemyData.enemyPrefab, pos);
                en.Hp = enemyData.forceHP;
                wst._enemiesList.Add(en);
                wst._spawnCounter++;
                yield return new WaitForSeconds(delay);
            }

            StartCoroutine(TrackSplitWave(wst));
        }

        IEnumerator TrackSplitWave(WaveSplitTracker wst)
        {

            if (wst._spawnCounter >= wst._totalRequired)
            {
                // Debug.Log("Track over, all enemies are spawned");
                yield return null;
            }
            else
            {
                bool stillHaveLiveEnemies = false;
                for (int i = 0; i < wst._enemiesList.Count; i++)
                {
                    if (wst._enemiesList[i].gameObject.activeSelf)
                    {
                        stillHaveLiveEnemies = true;
                        break;
                    }
                }
                if (stillHaveLiveEnemies)
                {
                    yield return new WaitForSeconds(_splitTrackDelay);
                    StartCoroutine(TrackSplitWave(wst));
                }
                else
                {
                    wst._enemiesList.Clear();
                    StartCoroutine(SpawnSplitOneByOne(wst, wst._enemyData, _spawnDelayPerEnemy));
                }

            }

        }

        IEnumerator SpawnOneByOne(WaveEnemyData enemyData, float delay)
        {
            for (int i = 0; i < enemyData.totalInWave; i++)
            {
                Vector3 pos = _spawnPositions._spawnList[Random.Range(0, _spawnPositions._spawnList.Length)];
                pos.x += Random.Range(-2.0f, 2.0f);
                pos.z += Random.Range(-2.0f, 2.0f);
                EnemyMain en = SpawnEnemy(enemyData.enemyPrefab, pos);
                en.Hp = enemyData.forceHP;
                yield return new WaitForSeconds(delay);
            }
        }

        EnemyMain SpawnEnemy(GameObject prefab, Vector3 position)
        {
            return Instantiate(prefab, position, Quaternion.identity).GetComponent<EnemyMain>();
        }



    }
}