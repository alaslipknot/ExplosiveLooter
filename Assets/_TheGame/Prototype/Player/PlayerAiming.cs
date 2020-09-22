


using HardBit.Universal.Animation;
using HardBit.Universal.Math;
using UnityEngine;
using HardBit.Enemies;
using System.Collections.Generic;

namespace HardBit.Player {
    public class PlayerAiming : MonoBehaviour {
        [SerializeField] private Transform _debugTarget;
        [Header("Performance")]
        [SerializeField] private int _everyXFrames;
        private int _xFrameCounter;
        [Header("Targeting settings")]
        [Space(10)]
        [SerializeField] private bool _canAim= true;
        [SerializeField] private float _sphereCastRadius = 5;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _viewAngleLimit = 45f;
        [Header("Targeting variables (hide later)")]
        [Space(10)]
        [SerializeField] private Transform _currentTarget;
        [SerializeField] private Transform[] _possibleTargets;
        [Header("Animation Settings")]
        [Space(10)]
        [SerializeField] private float _aimWeightSmoothness = 5f;
        [SerializeField] private float _unaimWeightSmoothness = 2f;

        //private & references
        private BoneFaceTarget _boneFace;
        private Transform _transform;
        private EnemyTracker _enTracker;
        private PlayerInfo _playerInfo;
        void Start()
        {
            CacheInnerReference();

            EventSubscription();
        }

        void CacheInnerReference()
        {
            _transform = this.transform;
            _boneFace = GetComponent<BoneFaceTarget>();
            _enTracker = EnemyTracker._instance;
            _playerInfo = GetComponent<PlayerInfo>();
        }

        void EventSubscription()
        {
            _playerInfo.OnDeathEvent += OnDeathDo;
        }


        void Update()
        {
            if (_canAim)
            {
                _xFrameCounter++;
                if (_xFrameCounter >= _everyXFrames)
                {
                    GetPossibleTargets();
                    FindCorrectTarget(_enTracker.ListOfEnemies);
                    AimAtCurrentTarget();
                }
            }
            else
            {
                _boneFace.Weight = 0;
            }
           

        }

        bool CheckIfTargetInFront(Transform target)
        {
            if (target == null) return false;
            return MathHelp.IsInFront(target.position, _transform.position, _transform.forward);
        }

        float GetAngleBetween(Transform target)
        {
            Vector3 targetDir = target.position - transform.position;
            Vector3 forward = transform.forward;
            float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);
            return angle;
        }

        void AimAtCurrentTarget()
        {

            if (_currentTarget != null)
            {
                _boneFace.LookAtTarget.parent = _currentTarget;
                _boneFace.LookAtTarget.localPosition = Vector3.Lerp(_boneFace.LookAtTarget.localPosition, Vector3.zero, _aimWeightSmoothness * Time.deltaTime);

                if (_boneFace.Weight < 1)
                {
                    _boneFace.Weight = Mathf.Lerp(_boneFace.Weight, 1.0f, _aimWeightSmoothness * Time.deltaTime);

                }
                else
                {
                    _boneFace.Weight = 1;
                }
            }
            else
            {

                _boneFace.LookAtTarget.parent = null;
                if (_boneFace.Weight < 1)
                {
                    _boneFace.Weight = Mathf.Lerp(_boneFace.Weight, 0.0f, _unaimWeightSmoothness * Time.deltaTime);
                }
                else
                {
                    _boneFace.Weight = 0;
                }
            }


        }

        void FindCorrectTarget(List<EnemyMain> _list)
        {
            if (_list.Count == 0)
            {
                _currentTarget = null;
                return;
            }

            _currentTarget = null;
            for (int i = 0; i < _list.Count; i++)
            {
                Transform t = _list[i].Transform;
                if (CheckIfTargetInFront(t)
                    && Mathf.Abs(GetAngleBetween(t)) <= _viewAngleLimit
                    && Vector3.Distance(t.position, _transform.position) <= _sphereCastRadius)
                {
                    float oldDistance = Mathf.Infinity;
                    if (_currentTarget != null)
                    {
                        oldDistance = Vector3.Distance(_currentTarget.position, _transform.position);
                    }
                    float currentDistance = Vector3.Distance(t.position, _transform.position);

                    if (currentDistance < oldDistance)
                    {
                        _currentTarget = _list[i].Transform;
                    }
                }

            }
        }

        void FindCorrectTarget()
        {
            if (_possibleTargets.Length == 0)
            {
                _currentTarget = null;
                return;

            }
            _currentTarget = null;
            for (int i = 0; i < _possibleTargets.Length; i++)
            {

                Transform t = _possibleTargets[i];
                if (CheckIfTargetInFront(t) && Mathf.Abs(GetAngleBetween(t)) <= _viewAngleLimit)
                {
                    float oldDistance = Mathf.Infinity;
                    if (_currentTarget != null)
                    {
                        oldDistance = Vector3.Distance(_currentTarget.position, _transform.position);
                    }
                    float currentDistance = Vector3.Distance(t.position, _transform.position);

                    if (currentDistance < oldDistance)
                    {
                        _currentTarget = _possibleTargets[i];
                    }
                }

            }
        }



        void GetPossibleTargets()
        {
            Collider[] hitColliders = Physics.OverlapSphere(_transform.position, _sphereCastRadius, _layerMask);
            _possibleTargets = new Transform[hitColliders.Length];
            for (int i = 0; i < _possibleTargets.Length; i++)
            {
                _possibleTargets[i] = hitColliders[i].transform;
            }
        }


        void OnDeathDo()
        {
            _canAim = false;
        }

        #region Gizmos & Debug

        void DrawLinesToTargets()
        {
            for (int i = 0; i < _possibleTargets.Length; i++)
            {
                if (_possibleTargets[i] == _currentTarget)
                {
                    Debug.DrawLine(_transform.position, _possibleTargets[i].position, Color.cyan);
                }
                else
                {
                    Debug.DrawLine(_transform.position, _possibleTargets[i].position, Color.red);
                }
            }
        }

        void DrawAngleToTarget()
        {

        }
        private void OnDrawGizmos()
        {
            DrawLinesToTargets();
            DrawAngleToTarget();
        }
        #endregion

    }
}
