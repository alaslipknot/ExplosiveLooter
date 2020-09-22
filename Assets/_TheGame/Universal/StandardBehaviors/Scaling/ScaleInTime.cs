using DG.Tweening;
using UnityEngine;

namespace HardBit.Universal.StandardBehaviors.Scaling
{
    public class ScaleInTime : MonoBehaviour
    {
        [SerializeField] private float _scale;
        [SerializeField] private float _time;
        [SerializeField] private float _delay;
        [SerializeField] private Ease _ease;


        public void ScaleThis()
        {
            transform.DOScale(_scale, _time).SetEase(_ease).SetDelay(_delay);
        }

    }
}
