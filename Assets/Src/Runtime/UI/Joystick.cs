using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Hypergame.UI
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public float movRadius = 100f;
        public GameObject knob;
        public UnityEvent<Vector2> getDelta;

        private Vector2 origin;
        private bool trackPointer;

        private RectTransform knobTransform => knob.transform as RectTransform;

        public void OnPointerDown(PointerEventData eventData)
        {
            trackPointer = true;
            origin = GetMousePosition();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            knob.transform.localPosition = Vector3.zero;
            getDelta?.Invoke(Vector2.zero);
            trackPointer = false;
        }

        private void Update()
        {
            if (trackPointer)
                SetPosition();
        }

        public void SetPosition()
        {
            Vector2 position = GetMousePosition();

            var target = CalcMoviment(position);
            print(target == position);
            knobTransform.anchoredPosition = target - origin;

            if (gameObject.activeSelf) getDelta?.Invoke((target - origin).normalized);
            else getDelta?.Invoke(Vector2.zero);
        }

        private Vector2 CalcMoviment(Vector2 position)
        {
            float dist = Vector2.Distance(origin, position);
            if (dist < movRadius) return position;

            Vector2 lenght = new Vector2()
            {
                x = position.x - origin.x,
                y = position.y - origin.y
            };

            float mag = Mathf.Sqrt(lenght.x * lenght.x + lenght.y * lenght.y);
            Vector2 result = new Vector2(
                origin.x + lenght.x / mag * movRadius,
                origin.y + lenght.y / mag * movRadius
            );

            return result;
        }

        private Vector2 GetMousePosition()
        {
#if UNITY_EDITOR_WIN
            return Input.mousePosition;
#else
            return Touchscreen.current.primaryTouch.position.ReadValue();
#endif
        }
    }
}
