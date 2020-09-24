using UnityEngine;
using UnityEngine.Events;

namespace HardBit.Specific.UserInput {

    public class InputHandler : MonoBehaviour {


        public static InputHandler _instance;

        [SerializeField] private Joystick _joysticLeft;
        [SerializeField] private Joystick _joysticRight;
        [SerializeField] private UnityEvent _shootingEvent;
        [SerializeField] private UnityEvent _shootingResetEvent;
        [SerializeField] private UnityEvent _flippingEvent;

        public Vector2 DirectionLeft { get => JoystickLeftDirection(); }
        public Vector2 DirectionRight { get => JoystickRightDirection(); }

        public delegate void OnInteractDelegate();
        public event OnInteractDelegate OnInteractEvent;



        private void OnEnable()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Shoot()
        {

            _shootingEvent.Invoke();
        }

        public void ShootReset()
        {
            _shootingResetEvent.Invoke();
        }

        public void Flip()
        {
            _flippingEvent.Invoke();
        }

        public void Interact()
        {
            OnInteractEvent?.Invoke();
        }



        public Vector2 JoystickLeftDirection()
        {

            var physJoy = new Vector2(Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
            return _joysticLeft.Direction + physJoy;

        }

        public Vector2 JoystickRightDirection()
        {

            var physJoy = new Vector2(Input.GetAxis("Horizontal2"), UnityEngine.Input.GetAxis("Vertical2"));
            return _joysticRight.Direction + physJoy;

        }

        private void Update()
        {
            //|| JoystickRightDirection().magnitude > 0
            if (Input.GetButton("Shoot") ) 
            {
                Shoot();
            }


            if (Input.GetButtonUp("Shoot"))
            {
                ShootReset();
            }

            if (Input.GetButtonDown("Flip"))
            {
                Flip();
            }

            if (Input.GetButtonDown("Interact"))
            {
                Interact();
            }
        }
    }
}
