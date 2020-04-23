﻿using System.Runtime.CompilerServices;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float horizontalPanSpeed = 100f;
    public float verticalPanSpeed = 100f;
    public float rotateSpeed;
    public float rotateFactor;
    private Quaternion _rotation;
    private float _minHeight = 10f;
    private float _maxHeight = 100f;
    private bool _cursorLocked = false;

    // Start is called before the first frame update
    void Start()
    {
        _rotation = Camera.main.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        RotateCamera();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Camera.main.transform.rotation = _rotation;
        }
    }

    private void RotateCamera()
    {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;
        if (Input.GetMouseButton(2))
        {
            if (!_cursorLocked) {
                Cursor.lockState = CursorLockMode.Locked;
                _cursorLocked = true;
            }
            var x = destination.x - Input.GetAxis("Mouse Y") * rotateFactor;
            destination.x = Mathf.Clamp(x, 0, 80);
            
            var yOrig = destination.y + Input.GetAxis("Mouse X") * rotateFactor; 
            if (yOrig < 270 && yOrig > 260)
            {
                destination.y = 270;
            } else if (yOrig > 90 && yOrig < 100)
            {
                destination.y = 90;
            }
            else
            {
                destination.y = yOrig;
            }
        } else
        {
            if (_cursorLocked) {
                Cursor.lockState = CursorLockMode.None;
                _cursorLocked = false;
            }
        }

        if (destination != origin)
        {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * rotateSpeed);
        }
    }

    private void MoveCamera()
    {
        var delta = Time.deltaTime;
        var currentPanSpeed = delta * horizontalPanSpeed;
        Camera camera = Camera.main;
        float moveX = camera.transform.position.x;
        float moveZ = camera.transform.position.z;
        float moveY = camera.transform.position.y;

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
        camera.transform.position = newPos;
    }
}