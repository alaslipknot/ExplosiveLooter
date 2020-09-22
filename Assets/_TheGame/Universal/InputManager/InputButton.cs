using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace HardBit.Universal.Input
{
    public class InputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [Header("Events")]
        #region Events
        [SerializeField] private UnityEvent ClickEvent;
        [SerializeField] private UnityEvent DownEvent;
        #endregion


        private bool pointerDown;

      

        #region Interface Contracts
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent.Invoke();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            pointerDown = true;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            Reset();
        }
        #endregion

        private void Update()
        {
            if (pointerDown)
            {
                DownEvent.Invoke();
            }
        }

        void Reset()
        {
            pointerDown = false;
        }
    }
}