using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
    using System;
    using System.Reflection;
    using UnityEditor;
#endif


namespace Kosu.UnityLibrary
{
    public class GestureInfo
    {
        public Vector2 DeltaPosition;

        public float DeltaScale;

        public int DisplayId;
    }

    public class DragGeture : UnityEvent<GestureInfo> { }

    public class PinchGesture : UnityEvent<GestureInfo> { }

    public class Scroll : UnityEvent<GestureInfo> { }

    public class DoublePress : UnityEvent<GestureInfo> { }

    public class RotateGesture : UnityEvent<GestureInfo> { }

    public class TouchManager : Singleton<TouchManager>
    {

        private Vector3 _lastDragPos;

        private Vector3 _dragPos;

        public DragGeture OnDrag = new DragGeture();

        public RotateGesture OnRotate = new RotateGesture();

        public PinchGesture OnPinch = new PinchGesture();

        public Scroll OnMouseWheelScroll = new Scroll();

        public DoublePress OnDoublePress = new DoublePress();

        private float _lastFingersDist;

        private float _fingersDist;

        private float _deltaTime;

        [SerializeField]
        private float _doublePressThresholdTime = 0;

        private int _pressCount;

        public override bool IsDontDestroyOnLoad
        {
            get
            {
                return true;
            }
        }

        protected override void AfterAwake()
        {
            base.AfterAwake();
        }

        protected override void AfterDestroy()
        {
            base.AfterDestroy();
            OnDrag.RemoveAllListeners();
            OnPinch.RemoveAllListeners();
            OnMouseWheelScroll.RemoveAllListeners();
            OnDoublePress.RemoveAllListeners();
        }

        private void Update()
        {
            if (_pressCount > 0)
            {
                _deltaTime += Time.deltaTime;
            }

            if (_deltaTime >= _doublePressThresholdTime)
            {
                _deltaTime = 0f;
                _pressCount = 0;
            }

            if (Input.touchCount > 1)
            {
                var t0 = Input.GetTouch(0);
                var t1 = Input.GetTouch(1);
                var tmp0 = RelativeMouseAt(t0.position);
                var tmp1 = RelativeMouseAt(t1.position);

                if (t1.phase == TouchPhase.Began)
                {
                    _lastFingersDist = Vector2.Distance(tmp0, tmp1);
                }
                else if (t0.phase == TouchPhase.Moved ||
                         t1.phase == TouchPhase.Moved)
                {
                    _fingersDist = Vector2.Distance(tmp0, tmp1);

                    if (tmp0.z == tmp1.z)
                    {
                        int displayId = (int) tmp0.z;
                        _OnPinch(displayId);
                    }
                }
            }
            else if (Input.touchCount > 0)
            {
                var t0 = Input.GetTouch(0);
                var pos = t0.position;

                switch (t0.phase)
                {
                    case TouchPhase.Began:
                        _lastDragPos = RelativeMouseAt(pos);
                        break;

                    case TouchPhase.Moved:
                        _dragPos = RelativeMouseAt(pos);
                        OnMoved();
                        _deltaTime = 0f;
                        _pressCount = 0;
                        break;

                    case TouchPhase.Ended:
                        _dragPos = Vector3.zero;
                        _lastDragPos = Vector3.zero;
                        _pressCount++;

                        if (_pressCount == 2 &&
                            _deltaTime < _doublePressThresholdTime)
                        {
                            _OnDoublePress();
                        }

                        break;

                    case TouchPhase.Stationary:
                    case TouchPhase.Canceled:
                        break;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var pos = Input.mousePosition;
                    _lastDragPos = RelativeMouseAt(pos);
                }

                if (Input.GetMouseButton(0))
                {
                    var pos = Input.mousePosition;
                    _dragPos = RelativeMouseAt(pos);
                    OnMoved();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    _dragPos = Vector3.zero;
                    _lastDragPos = Vector3.zero;
                }

                if (Input.GetMouseButtonDown(1))
                {
                    var pos = Input.mousePosition;
                    _lastDragPos = RelativeMouseAt(pos);
                }

                if (Input.GetMouseButton(1))
                {
                    var pos = Input.mousePosition;
                    _dragPos = RelativeMouseAt(pos);
                    _OnRotate();
                }

                if (Input.GetMouseButtonUp(1))
                {
                    _dragPos = Vector3.zero;
                    _lastDragPos = Vector3.zero;
                }

                float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

                if (mouseWheel > 0 ||
                    mouseWheel < 0)
                {
                    // mouse zoom
                    var pos = RelativeMouseAt(Input.mousePosition);
                    _OnMouseWheelScroll(mouseWheel, (int) pos.z);
                }
            }

            Vector3 mousePos = RelativeMouseAt(Input.mousePosition);

            if (mousePos.z == 0)
            {
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
            }
        }

        private void OnMoved()
        {
            var gesture = new GestureInfo();
            gesture.DeltaPosition = _lastDragPos - _dragPos;
            gesture.DisplayId = (int)_dragPos.z;

            if (OnDrag != null)
            {
                OnDrag.Invoke(gesture);
            }

            _lastDragPos = _dragPos;
        }

        private void _OnRotate()
        {
            var gesture = new GestureInfo();
            gesture.DeltaPosition = _lastDragPos - _dragPos;
            gesture.DisplayId = (int)_dragPos.z;

            if (OnRotate != null)
            {
                OnRotate.Invoke(gesture);
            }

            _lastDragPos = _dragPos;
        }

        private void _OnPinch(int displayId)
        {
            var gesture = new GestureInfo();
            gesture.DeltaScale = _lastFingersDist - _fingersDist;
            gesture.DisplayId = displayId;

            if (OnPinch != null)
            {
                OnPinch.Invoke(gesture);
            }

            _lastFingersDist = _fingersDist;
        }

        private void _OnMouseWheelScroll(float deltaScale, int displayId)
        {
            var gesture = new GestureInfo();
            gesture.DeltaScale = deltaScale;
            gesture.DisplayId = displayId;

            if (OnMouseWheelScroll != null)
            {
                OnMouseWheelScroll.Invoke(gesture);
            }

            _pressCount = 0;
            _deltaTime = 0f;
        }

        private void _OnDoublePress()
        {
            var gesture = new GestureInfo();

            if (OnDoublePress != null)
            {
                OnDoublePress.Invoke(gesture);
            }
        }

        private Vector3 RelativeMouseAt(Vector2 pos)
        {
#if UNITY_EDITOR
            var mouseOverWindow = EditorWindow.mouseOverWindow;
            Assembly assembly = typeof(EditorWindow).Assembly;
            Type type = assembly.GetType("UnityEditor.GameView");
            int displayID = 0;

            if (type.IsInstanceOfType(mouseOverWindow))
            {
                var displayField = type.GetField("m_TargetDisplay", BindingFlags.NonPublic | BindingFlags.Instance);
                displayID = (int)displayField.GetValue(mouseOverWindow);
            }

            var res = new Vector3(pos.x, pos.y, 0);
            res.z = displayID;
            return res;
#else
            return Display.RelativeMouseAt(pos);
#endif
        }


    }
}
