using System.Collections.Generic;
using UnityEngine;

namespace HardBit.Enemies
{
    public class EnemyGenerator : MonoBehaviour
    {

        [SerializeField] private int _number;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private List<EnemyMain> _enList;

        public List<EnemyMain> EnList { get => _enList; set => _enList = value; }

        public virtual void Generate()
        {
            for (int i = 0; i < _number; i++)
            {
                EnemyMain en = Create().GetComponent<EnemyMain>();
                en.gameObject.SetActive(false);
                EnList.Add(en);
            }
        }



        public virtual GameObject Create()
        {
            return Instantiate(_prefab, transform);
        }
    }
}
