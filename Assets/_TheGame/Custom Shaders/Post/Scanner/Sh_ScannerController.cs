using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[ExecuteInEditMode]
public class Sh_ScannerController : MonoBehaviour
{
    [SerializeField] private Material _scanMaterial;
    [SerializeField] private string _scaleID = "_Scale";
    [Range(0, 10)]
    [SerializeField] private float _scale;
    [SerializeField] private string _posID = "_WorldPosition";
    [SerializeField] private Vector4 _pos;
    [SerializeField] private string _powerID = "_Power";
    [Space(10)]
    [SerializeField] private float _animDuration = 0.25f;
    [SerializeField] private Ease _ease = Ease.InOutSine;

    void Start()
    {

    }

    void Update()
    {

        UpdateMaterial();
        if (Input.GetKeyDown(KeyCode.S))
        {
            TriggerEffect(10);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            TriggerEffect(0);
        }
    }

    private void UpdateMaterial()
    {
        if (_scanMaterial != null)
        {
            GetPropertyFromObjects();
            SetMaterialProperties();
        }
    }



    private void SetMaterialProperties()
    {
        _scanMaterial.SetFloat(_scaleID, _scale * 0.1f);
        _scanMaterial.SetFloat(_powerID, _scale * 0.1f);
        _scanMaterial.SetVector(_posID, _pos * -1);
    }

    private void GetPropertyFromObjects()
    {
        _pos = transform.position;
        transform.localScale = new Vector3(_scale, _scale, _scale);
    }

    private void TriggerEffect(float target)
    {
        DOTween.To(() => _scale, x => _scale = x, target, _animDuration).SetEase(_ease);

      
    }


}
