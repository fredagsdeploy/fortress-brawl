using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class CameraManager : MonoBehaviour
{
    public float horizontalPanSpeed = 100f;
    public float verticalPanSpeed = 100f;
    public float rotateSpeed = 50f;
    public float rotateFactor = 50f;
    private Quaternion _rotation;
    private float _minHeight = 10f;
    private float _maxHeight = 100f;
    private bool _cursorLocked = false;
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _rotation = _camera.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        RotateCamera();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _camera.transform.rotation = _rotation;
        }
    }
    
    private void RotateCamera()
    {
        Vector3 origin = _camera.transform.eulerAngles;
        Vector3 destination = origin;
        if (Input.GetMouseButton((int) MouseButton.MiddleMouse))
        {
            if (!_cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                _cursorLocked = true;
            }

            var x = destination.x - Input.GetAxis("Mouse Y") * rotateFactor;
            destination.x = Mathf.Clamp(x, 0, 80);

            var yOrig = destination.y + Input.GetAxis("Mouse X") * rotateFactor;
            if (yOrig < 270 && yOrig > 260)
            {
                destination.y = 270;
            }
            else if (yOrig > 90 && yOrig < 100)
            {
                destination.y = 90;
            }
            else
            {
                destination.y = yOrig;
            }
        }
        else
        {
            if (_cursorLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                _cursorLocked = false;
            }
        }

        if (destination != origin)
        {
            _camera.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * rotateSpeed);
        }
    }

    private void MoveCamera()
    {
        var delta = Time.deltaTime;
        var currentPanSpeed = delta * horizontalPanSpeed;
        float moveX = _camera.transform.position.x;
        float moveZ = _camera.transform.position.z;
        float moveY = _camera.transform.position.y;

        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        if (Input.GetKey(KeyCode.A))
        {
            moveX -= currentPanSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveX += currentPanSpeed;
        }

        if (Input.GetKey(KeyCode.W))
        {
            moveZ += currentPanSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveZ -= currentPanSpeed;
        }

        moveY -= Input.GetAxis("Mouse ScrollWheel") * currentPanSpeed * verticalPanSpeed;
        moveY = Mathf.Clamp(moveY, _minHeight, _maxHeight);
        Vector3 newPos = new Vector3(moveX, moveY, moveZ);
        _camera.transform.position = newPos;
    }
}