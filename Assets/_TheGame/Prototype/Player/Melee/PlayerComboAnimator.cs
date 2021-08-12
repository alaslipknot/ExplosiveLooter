
using UnityEngine;
using Animancer;

namespace HardBit.Player {
    public class PlayerComboAnimator : MonoBehaviour {
        // Start is called before the first frame update
        private Animator _anim;
        public int _comboID;
        public int _maxCombo = 2;
        public bool _doTimer;
        public bool _canHoldAttack = true;
        public bool _isInAttack;
        public bool _isPreparing;
        private float _comboTimer;
        public float _comboTimeThreshhold = 1;
        private float _holdtimeCounter;
        public float _holdTime = 0.5f;
        public ParticleSystem[] _slashParticle;
        public GameObject _damageCollider;

        void Start()
        {
            _anim = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Shoot"))
            {
                if (!_isPreparing)
                {
                    _isPreparing = true;
                    PrepareAttackTrigger(true);
                }
            }


            if (Input.GetButton("Shoot"))
            {


                if (!_isInAttack)
                {

                    if (_canHoldAttack)
                    {
                        _holdtimeCounter += Time.deltaTime;
                        if (_holdtimeCounter >= _holdTime)
                        {
                            PrepareAttackTrigger(false);
                            HoldAttackTrigger();
                            _canHoldAttack = false;
                            _comboID = 0;
                        }
                    }
                }
            }


            if (Input.GetButtonUp("Shoot"))
            {
               
                if (!_isInAttack)
                {
                    _comboID++;
                    if (_comboID > _maxCombo)
                    {
                        _comboID = 1;
                    }
                    SetMeleeComboID();
                    MeleeAttackTrigger();
                }
                _comboTimer = 0;
                _canHoldAttack = true;
                _holdtimeCounter = 0;
                _isPreparing = false;
                PrepareAttackTrigger(false);
            }




            CalculateComboTime();

        }




        #region Melee
        void SetMeleeComboID()
        {
            _anim.SetInteger("ComboID", _comboID);

        }

        void MeleeAttackTrigger()
        {
            _anim.SetTrigger("Attack");

        }

        void HoldAttackTrigger()
        {
            _anim.SetTrigger("HoldAttack");

        }


        void PrepareAttackTrigger(bool b)
        {
            _anim.SetBool("PrepareAttack", b);

        }

        void LeavePrepareAttackTrigger()
        {
            _anim.SetTrigger("LeavePrepare");

        }

        void ResetMeleeAnim()
        {
            _comboTimer = 0;
            _comboID = 0;
            _doTimer = false;
            _holdtimeCounter = 0;
            _isInAttack = false;
            SetMeleeComboID();
        }

        void CalculateComboTime()
        {
            _comboTimer += Time.deltaTime;

            if (_comboTimer >= _comboTimeThreshhold)
            {
                ResetMeleeAnim();
            }
        }

        #endregion


        public void PlaySlash(int id)
        {
            _slashParticle[id].Play();

        }
        public void SetDamage(int v)
        {
            _damageCollider.SetActive(v == 1);
            PlayerSingleton._instance._playerMovement.DoMeleePushForwad();
        }

        public void SetCanMove(int v)
        {
            PlayerSingleton._instance._playerMovement.SetCanMove(v);
        }

        public void SetInAttack(int v)
        {
            _isInAttack = v == 1;
        }
    }
}
