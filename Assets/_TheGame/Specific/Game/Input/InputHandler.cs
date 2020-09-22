using UnityEngine;
using UnityEngine.Events;

namespace HardBit.Specific.UserInput {

    public class InputHandler : MonoBehaviour {

        [SerializeField] private Joystick _joysticLeft;
        [SerializeField] private Joystick _joysticRight;
        [SerializeField] private UnityEvent _shootingEvent;
        [SerializeField] private UnityEvent _shootingResetEvent;
        [SerializeField] private UnityEvent _flippingEvent;

        public Vector2 DirectionLeft { get => JoystickLeftDirection(); }
        public Vector2 DirectionRight { get => JoystickRightDirection(); }




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
            if (Input.GetButton("Shoot") || JoystickRightDirection().magnitude > 0)
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
        }
    }
}
