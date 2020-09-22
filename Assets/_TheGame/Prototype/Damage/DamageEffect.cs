
using UnityEngine;

namespace HardBit.Specific.Gameplay
{
    public class DamageEffect : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private float _animSpeed = 0.5f;
        [SerializeField] private float _resetAfter = 0.2f;
        private float _resetCounter;
        private float _damageRatio;

        private void Awake()
        {
            this.enabled = false;
        }

        public void DoEffect()
        {
            this.enabled = true;
            _resetCounter = _resetAfter;
            SetDamage(1.0f);
            return;

        }



        void SetDamage(float damage)
        {
            _renderer.material.SetFloat("_DamageRatio", damage);
        }

        void ResetMaterial()
        {
            SetDamage(0);
            this.enabled = false;
        }

        private void Update()
        {
            if (_resetCounter > 0)
            {
                _resetCounter -= Time.deltaTime;
               
            }

            if (_resetCounter <= 0)
            {
                ResetMaterial();
            }
        }
    }

}
