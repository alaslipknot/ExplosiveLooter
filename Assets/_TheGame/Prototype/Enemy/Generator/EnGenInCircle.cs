
using UnityEngine;
using DG.Tweening;
using System.Collections;
using Random = UnityEngine.Random;

namespace HardBit.Enemies
{
    public class EnGenInCircle : EnemyGenerator
    {

        [SerializeField] private float _radius;
        [SerializeField] private float _waitingTime;
        [SerializeField] private bool _randomizePosition;

        private void Start()
        {
            Generate();

            Debug.Log("Message");
        }
        public override void Generate()
        {
            base.Generate();
            RandomizeThisPosition();
            StartCoroutine(GenSequence());
            StartCoroutine(CheckToRegenerate());

        }

        private void ReGen()
        {

        }
        private IEnumerator GenSequence()
        {
            for (int i = 0; i < EnList.Count; i++)
            {

                EnemyMain en = EnList[i];
                PlaceInCircle(en.transform);
                yield return new WaitForSeconds(_waitingTime);

                Setup(en);
                Appear(en.transform);
            }
        }


        private IEnumerator CheckToRegenerate()
        {
            yield return new WaitForSeconds(1.0f);
            if (IsAllDead())
            {
                ReGen();
            }
            else
            {
                StartCoroutine(CheckToRegenerate());
            }
        }


        bool IsAllDead()
        {
            for (int i = 0; i < EnList.Count; i++)
            {
                if (EnList[i].gameObject.activeSelf)
                {
                    return false;
                }
            }

            return true;
        }

        void RandomizeThisPosition()
        {
            var pos = Vector3.zero;
            pos.z = Random.Range(-10, 10);
            pos.x = Random.Range(-10, 10);
            pos.y = transform.position.y;

            transform.position = pos;
        }

        void PlaceInCircle(Transform t)
        {

            t.position = RandomCircle(transform.position, _radius);

        }

        void Appear(Transform t)
        {
            t.localScale = Vector3.zero;
            t.DOScale(1, 0.25f).SetEase(Ease.OutBounce);
        }


        void Setup(EnemyMain e)
        {
            e.gameObject.SetActive(true);
        }

        Vector3 RandomCircle(Vector3 center, float radius)
        {
            float ang = Random.value * 360;
            Vector3 pos;
            pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = center.y;
            pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            return pos;
        }

    }
}
