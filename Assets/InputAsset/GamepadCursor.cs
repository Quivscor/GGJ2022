using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public static class GeneralizedCursor
{
    public static Vector2 cursorPosition;
}

public class GamepadCursor : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private RectTransform cursorTransform;
    [SerializeField] private RectTransform canvasTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float cursorSpeed;
    [SerializeField] private float padding;

    private bool _previousMouseState;
    private Camera _camera;
    private Controls _controls;
    private Mouse _virtualMouse;
    private Mouse _currentMouse;

    private string _previousControlScheme = "";
    private const string gamepadScheme = "Gamepad";
    private const string keyboardScheme = "Keyboard&Mouse";

    private bool _isClampCursorToCircle = true;
    [SerializeField] private float _clampRadius;
    private Vector2 _centerPosition;

    private void OnEnable()
    {
        _controls = new Controls();
        _controls.Enable();
        _currentMouse = Mouse.current;
        _camera = Camera.main;
        _centerPosition = new Vector2(Screen.width / 2, Screen.height / 2);

        if(_virtualMouse == null)
        {
            _virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if(!_virtualMouse.added)
        {
            InputSystem.AddDevice(_virtualMouse);
        }

        InputUser.PerformPairingWithDevice(_virtualMouse, _input.user);
        //if(cursorTransform != null)
        //{
        //    Vector2 position = cursorTransform.anchoredPosition;
        //    InputState.Change(_virtualMouse.position, position);
        //}

        InputSystem.onAfterUpdate += UpdateMotion;
    }

    private void OnDisable()
    {
        if (_virtualMouse != null && !_virtualMouse.added)
        {
            InputSystem.RemoveDevice(_virtualMouse);
        }
        InputSystem.onAfterUpdate -= UpdateMotion;
    }

    private void Update()
    {
        if (_previousControlScheme != _input.currentControlScheme)
            OnControlsChanged(_input);

        _previousControlScheme = _input.currentControlScheme;
    }

    private void UpdateMotion()
    {
        if (_virtualMouse == null || Gamepad.current == null)
            return;

        Vector2 deltaValue = _controls.UI.Navigate.ReadValue<Vector2>();
        Debug.Log(deltaValue);
        deltaValue *= cursorSpeed * Time.deltaTime;

        Vector2 currentPosition = _virtualMouse.position.ReadValue();
        Vector2 newPosition = currentPosition + deltaValue;

        if(_isClampCursorToCircle)
        {
            float distance = Vector2.Distance(newPosition, _centerPosition);
            if(distance > _clampRadius)
            {
                Vector2 directionToCursor = newPosition - _centerPosition;
                directionToCursor *= _clampRadius / distance;
                newPosition = _centerPosition + directionToCursor;
            }
        }
        else
        {
            newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
            newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);
        }

        InputState.Change(_virtualMouse.position, newPosition);
        InputState.Change(_virtualMouse.delta, deltaValue);

        bool mousePressed = _controls.UI.Click.IsPressed();
        if (_previousMouseState != mousePressed)
        {
            _virtualMouse.CopyState<MouseState>(out var mouseState);

            mouseState.WithButton(MouseButton.Left, mousePressed);
            InputState.Change(_virtualMouse, mouseState);
            _previousMouseState = mousePressed;
        }

        AnchorCursor(newPosition);
    }

    private void OnControlsChanged(PlayerInput input)
    {
        if(input.currentControlScheme == keyboardScheme && _previousControlScheme != keyboardScheme)
        {
            //cursorTransform.gameObject.SetActive(false);
            //Cursor.visible = true;
            _currentMouse.WarpCursorPosition(_virtualMouse.position.ReadValue());
            Debug.Log("Controls changed to " + keyboardScheme);
        }
        else if(input.currentControlScheme == gamepadScheme && _previousControlScheme != gamepadScheme)
        {
            //cursorTransform.gameObject.SetActive(true);
            //Cursor.visible = false;
            InputState.Change(_virtualMouse.position, _currentMouse.position.ReadValue());
            AnchorCursor(_currentMouse.position.ReadValue());
            Debug.Log("Controls changed to " + gamepadScheme);
        }
    }

    private void AnchorCursor(Vector2 position)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, position, 
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _camera, out anchoredPosition);
        //cursorTransform.anchoredPosition = anchoredPosition;
    }
}
