using System.Collections.Generic;
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
    
    
    private Vector3 startScreenPos;
    private BoxCollider worldCollider;
    private RectTransform rt;
    private bool isSelecting;

    private void Start()
    {
        if (canvas == null)
            canvas = FindObjectOfType<Canvas>();
 
        if (selectionBox != null)
        {
            //We need to reset anchors and pivot to ensure proper positioning
            rt = selectionBox.GetComponent<RectTransform>();
            rt.pivot = Vector2.one * .5f;
            rt.anchorMin = Vector2.one * .5f;
            rt.anchorMax = Vector2.one * .5f;
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
        selectionBox.gameObject.SetActive(isSelecting);
        if (isSelecting)
        {
            Bounds b = new Bounds
            {
                center = Vector3.Lerp(startScreenPos, Input.mousePosition, 0.5f),
                size = new Vector3(Mathf.Abs(startScreenPos.x - Input.mousePosition.x),
                    Mathf.Abs(startScreenPos.y - Input.mousePosition.y),
                    0)
            };
            //The center of the bounds is inbetween startpos and current pos
            //We make the size absolute (negative bounds don't contain anything)

            //To display our selectionbox image in the same place as our bounds
            rt.position = b.center;
            rt.sizeDelta = canvas.transform.InverseTransformVector(b.size);
            


            //Looping through all the selectables in our world (automatically added/removed through the Selectable OnEnable/OnDisable)
            foreach (Selectable selectable in selectables)
            {
                //If the screenPosition of the worldobject is within our selection bounds, we can add it to our selection
                Vector3 screenPos = Camera.main.WorldToScreenPoint(selectable.transform.position);
                screenPos.z = 0;
                selectable.isSelected = b.Contains(screenPos);
            }
        }
    }

    private void LeftMouseDown()
    {
        //Storing these variables for the selectionBox
        startScreenPos = Input.mousePosition;
        isSelecting = true;
    }

    private void LeftMouseUp()
    {
        isSelecting = false;
    }

    private void RightMouse()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit))
        {
            var tag = hit.collider.tag;
            if (tag == "Ground")
            {
                foreach (var selectable in selectables)
                {
                    if (selectable.isSelected)
                    {
                        selectable.SetDestination(hit.point);
                    }
                }
            }
        }
    }
}