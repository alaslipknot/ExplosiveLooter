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

        private WaveMaster _waveMaster;

        EnemySpawnPositions _spawnPositions;

        private void Start()
        {
            _spawnPositions = EnemySpawnPositions._instance;
            _waveMaster = transform.parent.GetComponent<WaveMaster>();
            LaunchWave();
        }

        #region startup
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

            StartCoroutine(SpawnSplitOneByOne(MakeSplitTracker(index, enemyData), enemyData, _spawnDelayPerEnemy));

        }
        #endregion

        #region Status

        public bool NoMoreSplits()
        {
            return _splitList.Count == 0;
        }

        #endregion

        #region Spawn & track multiple enemies
        IEnumerator SpawnSplitOneByOne(WaveSplitTracker wst, WaveEnemyData enemyData, float delay)
        {
            for (int i = 0; i < wst._splitNum; i++)
            {
                EnemyMain en = SpawnAndSetupEnemy(enemyData.enemyPrefab, enemyData);
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
                _splitList.Remove(wst);
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

        WaveSplitTracker MakeSplitTracker(int index, WaveEnemyData enemyData)
        {
            WaveSplitTracker wst = new WaveSplitTracker();
            wst._enemyData = enemyData;
            if (enemyData.splitWave)
            {
                wst._splitNum = enemyData.splitNum;
                wst._totalRequired = enemyData.totalInWave;
            }
            else
            {
                wst._splitNum = enemyData.totalInWave;
                wst._totalRequired = enemyData.totalInWave;
            }

            _splitList.Add(wst);
            return wst;
        }

        #endregion

        #region Init & setup single enemy
        EnemyMain SpawnAndSetupEnemy(GameObject prefab, WaveEnemyData data)
        {
            EnemyMain en = Instantiate(prefab).GetComponent<EnemyMain>();
            SetupEnemy(en, data);
            return en;
        }

        void SetupEnemy(EnemyMain en, WaveEnemyData data)
        {
            //setup main data
            en.Hp = data.forceHP;
            en.OnDeathEvent += _waveMaster.OnEnemyDeath;
            EnMover enMover = en.GetComponent<EnMover>();
            enMover.MaxSpeed = data.forceSpeed;
            if (data.randomizeSpeed)
            {
                int x = (Random.Range(0, 2) == 0) ? 1 : -1;
                enMover.MaxSpeed += Random.Range(0, data.speedRandom) * x;
            }
            //setup position
            Vector3 pos = _spawnPositions._spawnList[Random.Range(0, _spawnPositions._spawnList.Length)];
            pos.x += Random.Range(-2.0f, 2.0f);
            pos.z += Random.Range(-2.0f, 2.0f);
            en.transform.position = pos;
        }
        #endregion



    }
}




/*  IEnumerator SpawnOneByOne(WaveEnemyData enemyData, float delay)
  {
      for (int i = 0; i < enemyData.totalInWave; i++)
      {
          EnemyMain en = SpawnAndSetupEnemy(enemyData.enemyPrefab, enemyData);
          yield return new WaitForSeconds(delay);
      }
  }*/