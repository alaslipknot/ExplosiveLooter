using HardBit.Enemies;
using System;
using System.Collections.Generic;

namespace HardBit.WaveSystem {

    [Serializable]
    public class WaveSplitTracker {

        public WaveEnemyData _enemyData; //the object itself in the waveSpec list
        public int _splitNum;       // number of sub-wave enemies
        public int _totalRequired;  // total enemies required based on WaveSpec list
        public int _spawnCounter;   // spawned enemies so far
        public List<EnemyMain> _enemiesList = new List<EnemyMain>(); // list of current enemies in the scene to keep track of
    }
}