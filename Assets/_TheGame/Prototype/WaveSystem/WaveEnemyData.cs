/*
Data required for enemies in the wave
Used by WaveSpec (Scriptable Object)
*/
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[Serializable]
public class WaveEnemyData {
    [HorizontalGroup("Enemy Data", 75)]
    [PreviewField(75)]
    [HideLabel]

    public GameObject enemyPrefab;
    [VerticalGroup("Enemy Data/Stats")]
    [LabelWidth(100)]
    public string name;
    [VerticalGroup("Enemy Data/Stats")]
    [LabelWidth(100)]
    [Range(0, 100)]
    public int totalInWave = 1;
    [VerticalGroup("Enemy Data/Stats")]
    [LabelWidth(100)]
    [Range(0, 100)]
    public int forceHP = 1;
    [VerticalGroup("Enemy Data/Stats")]
    [LabelWidth(100)]
    public bool splitWave = false;
    [VerticalGroup("Enemy Data/Stats")]
    [LabelWidth(100)]
    [ShowIf("splitWave")]
    [PropertyRange(1, "totalInWave")]
    [InfoBox("Send chosen x of the wave, send next when the initial x are all dead")]
    public int splitNum = 1;
}
