//using Commons;
//using UnityEngine;
//using UnityEngine.EventSystems;

//namespace Game.UI
//{
//    public class MobileJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
//    {
//        public Transform handle;
//        public float joystickRadius = 100f;
//        private Vector2 inputVector = Vector2.zero;

//        public Vector2 JoystickInput => inputVector;

//        private Vector3 startPos;

//        private void Awake()
//        {
//            if (handle == null)
//            {
//                handle = transform.GetChild(0);
//            }
//            startPos = handle.localPosition;
//        }

//        public void OnPointerDown(PointerEventData eventData)
//        {
//            if(GameManager.Instance.logJoystick) LogUtility.Info("JoysticK OnPointerDown");
//            //OnDrag(eventData);
//        }

//        public void OnDrag(PointerEventData eventData)
//        {
//            if (handle == null)
//            {
//                if (GameManager.Instance.logJoystick) LogUtility.Info("JoysticK OnDrag handle null");
//                return;
//            }
//            if (EventSystem.current == null)
//            {
//                LogUtility.Error("MobileJoystick: No EventSystem found in scene!");
//            }

//            if (GameManager.Instance.logJoystick) LogUtility.Info("JoysticK OnDrag");
//            Vector2 position;

//            RectTransformUtility.ScreenPointToLocalPointInRectangle(
//                (RectTransform)transform,
//                eventData.position,
//                eventData.pressEventCamera != null ? eventData.pressEventCamera : null,
//                out position
//            );
//            if(eventData.pressEventCamera != null)
//            {
//                LogUtility.Error("MobileJoystick: eventData.pressEventCamera != null");
//            }

//            Vector2 direction = position - (Vector2)startPos;
//            inputVector = Vector2.ClampMagnitude(direction / joystickRadius, 1f);
//            handle.localPosition = startPos + (Vector3)(inputVector * joystickRadius);
//        }

//        public void OnPointerUp(PointerEventData eventData)
//        {
//            if (handle == null)
//            {
//                if (GameManager.Instance.logJoystick) LogUtility.Info("JoysticK OnPointerUp handle null");
//                return;
//            }
//            if (GameManager.Instance.logJoystick) LogUtility.Info("JoysticK OnPointerUp");
//            inputVector = Vector2.zero;
//            handle.localPosition = startPos;
//        }

//        private void Update()
//        {
//            if (GameManager.Instance.logJoystickUpdate) LogUtility.Info("JoysticK Update", JoystickInput.ToString());
//        }
//    }
//}

using Commons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using UnityEngine.Windows;

namespace Game.UI
{

    public class MobileJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public Transform joystickHandle = null;
        [SerializeField] private Vector2 _startPos = Vector2.zero;
        private bool _isDragging = false;
        private Vector2 _input;
        private float _joystickRadius = 100f;


        private void Start()
        {
            if(joystickHandle == null && transform.childCount > 0)
            {
                joystickHandle = transform.GetChild(0);
                RectTransform rectTransform = GetComponent<RectTransform>();
                _startPos = rectTransform.anchoredPosition;
            }
            Debug.Log($"Drag Test Initialized: {joystickHandle != null}");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _startPos = GetPointerPosition(eventData);
            _isDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging) return;

            Vector2 currentPos = GetPointerPosition(eventData);
            Vector2 direction = currentPos - _startPos;
            _input = Vector2.ClampMagnitude(direction / _joystickRadius, 1f);
            joystickHandle.localPosition = (_input * _joystickRadius);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isDragging = false;
            _input = Vector2.zero;
            joystickHandle.localPosition = Vector2.zero;
        }

        private Vector2 GetPointerPosition(PointerEventData eventData)
        {
            return eventData.position;
        }
        public Vector2 JoystickInput { 
            get
            {
                return _input;
            }
        }



    }

}