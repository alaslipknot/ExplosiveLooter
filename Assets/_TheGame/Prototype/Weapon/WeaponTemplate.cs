
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponTemplate", menuName = "Ala/WeaponTemplate", order = 0)]
public class WeaponTemplate : ScriptableObject
{

    public BulletTemplate BulletTemplate;
    public int Damage;
    public float Cooldown;
    
}
