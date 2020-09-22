

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace HardBit.Player {
    public class PlayerHUD : MonoBehaviour {

        #region HP HUD
        [SerializeField] private Image _hpBarImage;
        [SerializeField] private Text _hpText;
        [SerializeField] private float _hpRatio;
        [SerializeField] private Gradient _hpBarColor;

        [Header("DoTween settings")]
        [SerializeField] private Ease _imgFillEase;
        [SerializeField] private Ease _txtChangeEase;
        [SerializeField] private float _imgFillTime;
        [SerializeField] private float _txtChangeTime;
        [SerializeField] private float _txtScaleMultiplier = 1.5f;
        Sequence _sequence;
        public void UpdateHpHud()
        {
            if (_sequence != null)
            {
                _sequence.Kill();
                _hpText.transform.localScale = new Vector3(1, 1, 1);
            }

            _sequence = DOTween.Sequence();
            Tween txt = _hpText.transform.DOScale(_txtScaleMultiplier, _txtChangeTime).SetEase(_txtChangeEase).SetLoops(2, LoopType.Yoyo);
            Tween imgFill = _hpBarImage.DOFillAmount(_hpRatio, _imgFillTime).SetEase(_imgFillEase);
            Tween imgCol = _hpBarImage.DOColor(GetColorFromGradient(_hpRatio), _imgFillTime).SetEase(_imgFillEase);

            _sequence.Append(txt).Append(imgFill).Append(imgCol);
            _sequence.Play();

        }

        public void SetHpInfo(float hpRatio, int hp, int maxHp)
        {
            _hpRatio = hpRatio;
            _hpText.text = hp + "/" + maxHp;
        }

        Color GetColorFromGradient(float ratio)
        {
            return _hpBarColor.Evaluate(ratio);
        }
        #endregion
    }
}