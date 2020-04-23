using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    public float panSpeed;
    public float panDetect = 15f;
    public float rotateSpeed;
    public float rotateFactor;
    private Quaternion _rotation;
    private float _minHeight = 10f;
    private float _maxHeight = 100f;
    
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
    }

    private void RotateCamera()
    {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;
        if (Input.GetMouseButtonDown(2))
        {
            destination.x -= Input.GetAxis("Mouse Y") * rotateFactor;
            destination.y += Input.GetAxis("Mouse X") * rotateFactor;
        }

        if (destination != origin)
        {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * rotateSpeed);
        }
    }

    private void MoveCamera()
    {
        var delta = Time.deltaTime;
        var currentPanSpeed = delta * panSpeed;
        Camera camera = Camera.main;
        float moveX = camera.transform.position.x;
        float moveZ = camera.transform.position.z;
        float moveY = camera.transform.position.y;

        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;
        
        if (Input.GetKey(KeyCode.A)  )
        {
            moveX -= currentPanSpeed;
        } else if (Input.GetKey(KeyCode.D))
        {
            moveX += currentPanSpeed;
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            moveZ += currentPanSpeed;
        } else if (Input.GetKey(KeyCode.S))
        {
            moveZ -= currentPanSpeed;
        }

        
        Vector3 newPos = new Vector3(moveX, moveY, moveZ);
        camera.transform.position = newPos;
    }
}
