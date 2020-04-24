﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class SelectionManager : MonoBehaviour
{
    public static List<Selectable> selectables = new List<Selectable>();
    
    [Tooltip("Canvas is set automatically if not set in the inspector")]
    public Canvas canvas;
    [Tooltip("The Image that will function as the selection box to select multiple objects at the same time. Without this you can only click to select.")]
    public Image selectionBox;
    
    
    private Vector3 _startScreenPos;
    private BoxCollider _worldCollider;
    private RectTransform _rt;
    private bool _isSelecting;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        if (canvas == null)
            canvas = FindObjectOfType<Canvas>();
 
        if (selectionBox != null)
        {
            //We need to reset anchors and pivot to ensure proper positioning
            _rt = selectionBox.GetComponent<RectTransform>();
            _rt.pivot = Vector2.one * .5f;
            _rt.anchorMin = Vector2.one * .5f;
            _rt.anchorMax = Vector2.one * .5f;
            selectionBox.gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown((int) MouseButton.LeftMouse))
        {
            LeftMouseDown();
        } 
        else if (Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
        {
            LeftMouseUp();
        }

        UpdateSelectionBox();
        
        if (Input.GetMouseButton((int) MouseButton.RightMouse))
        {
            RightMouse();
        }
    }

    private void UpdateSelectionBox()
    {
        selectionBox.gameObject.SetActive(_isSelecting);
        if (_isSelecting)
        {
            Bounds b = new Bounds
            {
                center = Vector3.Lerp(_startScreenPos, Input.mousePosition, 0.5f),
                size = new Vector3(Mathf.Abs(_startScreenPos.x - Input.mousePosition.x),
                    Mathf.Abs(_startScreenPos.y - Input.mousePosition.y),
                    0)
            };
            //The center of the bounds is inbetween startpos and current pos
            //We make the size absolute (negative bounds don't contain anything)

            //To display our selectionbox image in the same place as our bounds
            _rt.position = b.center;
            _rt.sizeDelta = canvas.transform.InverseTransformVector(b.size);
            


            //Looping through all the selectables in our world (automatically added/removed through the Selectable OnEnable/OnDisable)
            foreach (Selectable selectable in selectables)
            {
                //If the screenPosition of the worldobject is within our selection bounds, we can add it to our selection
                Vector3 screenPos = _camera.WorldToScreenPoint(selectable.transform.position);
                screenPos.z = 0;
                selectable.IsSelected = b.Contains(screenPos);
            }
        }
    }

    private void LeftMouseDown()
    {
        //Storing these variables for the selectionBox
        _startScreenPos = Input.mousePosition;
        _isSelecting = true;
    }

    private void LeftMouseUp()
    {
        _isSelecting = false;
    }

    private void RightMouse()
    {
        Ray r = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                foreach (var selectable in selectables)
                {
                    if (selectable.IsSelected)
                    {
                        selectable.SetDestination(hit.point);
                    }
                }
            }
        }
    }
}