using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletTemplate", menuName = "Ala/BulletTemplate", order = 1)]
public class BulletTemplate : ScriptableObject
{
    public float _speed;
    public GameObject _projectile;
    public GameObject _flash;
    public GameObject _impact;

}
