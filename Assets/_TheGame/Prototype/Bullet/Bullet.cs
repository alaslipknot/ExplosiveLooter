using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] 
    private BulletTemplate _bulletTemplate;
    private float _speed;
    private Transform _transform;
    private GameObject _projectile;
    private GameObject _flash;
    private GameObject _impact;
    private bool _canMove;
    void Start()
    {
        _transform = this.transform;
        SetupBullet();
        Shot();
    }


    void Update()
    {
        if (_canMove)
            _transform.Translate(_transform.forward * _speed * Time.deltaTime, Space.World);
    }

    void SetupBullet()
    {
        _flash = CreateElement(_bulletTemplate._flash);
        _projectile = CreateElement(_bulletTemplate._projectile);
        _impact = CreateElement(_bulletTemplate._impact);
        _speed = _bulletTemplate._speed;
        _canMove = false;
    }

    GameObject CreateElement(GameObject prefab)
    {
        GameObject go = Instantiate(prefab, _transform);
        go.SetActive(false);
        go.transform.localEulerAngles = Vector3.zero;
        return go;
    }


    void Shot()
    {
        _canMove = true;
        _flash.transform.parent = null;
        _flash.SetActive(true);

        _projectile.SetActive(true);

    }
    void Impact()
    {
        _canMove = false;
        _projectile.SetActive(false);
        _impact.SetActive(true);
        _flash.transform.parent = _transform;
        Invoke("Disable", 1.0f);
    }


    void Disable()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Impact();
    }
}
