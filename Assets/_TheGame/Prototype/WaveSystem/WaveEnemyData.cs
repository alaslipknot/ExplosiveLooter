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
    [Range(0, 100)]
    public int totalInWave = 1;
    [VerticalGroup("Enemy Data/Stats")]
    [LabelWidth(100)]
    [Range(0, 100)]
    public int forceHP = 1;
    [VerticalGroup("Enemy Data/Stats")]
    [LabelWidth(100)]
    [Range(0, 100)]
    public int forceSpeed = 5;
    [VerticalGroup("Enemy Data/Stats")]
    [LabelWidth(100)]
    public bool randomizeSpeed = false;
    [ShowIf("randomizeSpeed")]
    [PropertyRange(0, "forceSpeed")]
    [InfoBox("will add or remove a value between Zero and this to the enemy speed ")]
    [VerticalGroup("Enemy Data/Stats")]
    public int speedRandom = 1;

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
