using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace HardBit.Enemies {
    public class EnemyMain : MonoBehaviour {
        [Header("Parameters")]
        [SerializeField] private string _name;
        [SerializeField] private int _hp;
        [SerializeField] private int _attackPow;

        [Space(10)]

        [Header("Events")]
        [SerializeField] private UnityEvent _attackEvent;
        [SerializeField] private UnityEvent _deathEvent;
        [SerializeField] private UnityEvent _takeDamageEvent;

        [Header("Cached")]
        private Transform _transform;
        private EnemyTracker _enTracker;
        private Rigidbody _body;

        #region Delegates
        public delegate void OnDeathDelegate();
        public OnDeathDelegate OnDeathEvent;

        #endregion


        #region Properties
        public string Name { get => _name; set => _name = value; }
        public int Hp { get => _hp; set => _hp = value; }

        public int AttackPow { get => _attackPow; set => _attackPow = value; }
        public UnityEvent AttackEvent { get => _attackEvent; set => _attackEvent = value; }
        public UnityEvent DeathEvent { get => _deathEvent; set => _deathEvent = value; }
        public UnityEvent TakeDamageEvent { get => _takeDamageEvent; set => _takeDamageEvent = value; }
        public Transform Transform { get => _transform; set => _transform = value; }
        public Rigidbody Body { get => _body; set => _body = value; }
        #endregion

        private void OnEnable()
        {
            AddToTracker();
        }

        private void OnDisable()
        {
        }

        private void OnDestroy()
        {

        }

        private void Start()
        {
            CacheInnerVariables();
        }

        void CacheInnerVariables()
        {
            _transform = this.transform;
            _body = GetComponent<Rigidbody>();
        }


        public virtual void Attack() { }
        public virtual void Die()
        {
            RemoveFromTracker();
            OnDeathEvent?.Invoke();

        }
        public virtual void TakeDamage() { }

        #region Tracking
        void AddToTracker()
        {
            if (_enTracker == null)
            {
                StartCoroutine(GetTracker());
            }
            else
            {

                _enTracker.ListOfEnemies.Add(this);
            }
        }

        void RemoveFromTracker()
        {
            _enTracker.ListOfEnemies.Remove(this);
        }

        IEnumerator GetTracker()
        {
            yield return new WaitForSeconds(0.1f);
            _enTracker = EnemyTracker._instance;
            if (_enTracker != null)
            {
                AddToTracker();
            }
            else
            {
                Debug.LogError("Multiple Tracker check from " + gameObject.name);
                StartCoroutine(GetTracker());
            }

        }
        #endregion

    }
}
