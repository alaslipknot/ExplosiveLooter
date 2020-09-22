/*
 * Scriptable object that contains a list of WaveEnemyData.
 * Used by WaveGenerator to spawn required enemies per wave.
*/

using UnityEngine;
using Sirenix.OdinInspector;

namespace HardBit.WaveSystem {


    [CreateAssetMenu(fileName = "New Wave", menuName = "Ala/Wave", order = 1)]
    public class WaveSpec : ScriptableObject {
        [ListDrawerSettings(AddCopiesLastElement = true, NumberOfItemsPerPage = 10, ShowIndexLabels = true, ShowPaging = true)]
        public WaveEnemyData[] enemies;
    }



}