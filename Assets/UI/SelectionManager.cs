using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class SelectionManager : MonoBehaviour
{
    public float bottomUiHeight = 200;
    public float bottomUiWidth = 1200;
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
    private BottomUiController _bottomUiController;

    private void Awake()
    {
        _camera = Camera.main;
        _bottomUiController = canvas.GetComponentInChildren<BottomUiController>();
    }

    private void Start()
    {
        
 
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
        var shouldUpdateSelectionBox = true;
        if (Input.GetMouseButtonDown((int) MouseButton.LeftMouse))
        {
             shouldUpdateSelectionBox = LeftMouseDown();
        } 
        else if (Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
        {
            LeftMouseUp();
        }

        if (shouldUpdateSelectionBox)
        {
            UpdateSelectionBox();
        }
        
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
            bool changed = false;
            foreach (Selectable selectable in selectables)
            {
                //If the screenPosition of the worldobject is within our selection bounds, we can add it to our selection
                Vector3 screenPos = _camera.WorldToScreenPoint(selectable.transform.position);
                screenPos.z = 0;
                bool newValue = b.Contains(screenPos);
                if (newValue != selectable.IsSelected)
                {
                    selectable.IsSelected = newValue;
                    changed = true;
                }
                 
            }

            if (changed)
            {
                NotifySelectionUpdateToUI();
            }
            
        }
    }

    private bool LeftMouseDown()
    {
        float mouseX = Input.mousePosition.x;
        float center = (float) Screen.width / 2;
        float leftUi = center - bottomUiWidth / 2;
        float rightUi = center + bottomUiWidth / 2;
        float mouseY = Input.mousePosition.y;
        if (mouseY < bottomUiHeight && mouseX > leftUi && mouseX < rightUi)
        {
            return false;
        }
        
        Ray mouseToWorldRay = _camera.ScreenPointToRay(Input.mousePosition);
        //Shoots a ray into the 3D world starting at our mouseposition
        if (Physics.Raycast(mouseToWorldRay, out var hitInfo))
        {
            //We check if we clicked on an object with a Selectable component
            Selectable selectable = hitInfo.collider.GetComponentInParent<Selectable>();
            if (selectable)
            {
                var current = selectable.IsSelected;
                selectables.ForEach(s => s.IsSelected = false);
                selectable.IsSelected = true;
                if (!current)
                {
                    NotifySelectionUpdateToUI();
                }
                
                //If we clicked on a Selectable, we don’t want to enable our SelectionBox
                return false;
            }
        }
        
        //Storing these variables for the selectionBox
        _startScreenPos = Input.mousePosition;
        _isSelecting = true;
        return true;
    }

    private void LeftMouseUp()
    {
        _isSelecting = false;
    }

    private void RightMouse()
    {
        Ray r = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out var hit))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                foreach (var selectable in selectables.Where(selectable => selectable.IsSelected))
                {
                    selectable.SendMessage(nameof(IMovable.SetDestination), hit.point, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }

    private void NotifySelectionUpdateToUI()
    {
        _bottomUiController
            .OnUpdatedSelection(
                selectables.FindAll(s => s.IsSelected)
            );
    }
}