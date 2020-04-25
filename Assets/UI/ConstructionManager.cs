using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class ConstructionManager : MonoBehaviour
{
    private bool _isPlacing = false;

    public GameObject ghostBuildingPrefab;
    public GameObject buildingPrefab;

    private GameObject _ghostBuilding;
    private GhostBuildingManager _ghostBuildingManager;
    private MeshRenderer _ghostBuildingRenderer;

    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPlacing && Input.GetKeyDown(KeyCode.B))
        {
            _isPlacing = true;
            _ghostBuilding = Instantiate(ghostBuildingPrefab, transform, true);
            _ghostBuildingRenderer = _ghostBuilding.GetComponentInChildren<MeshRenderer>();
            _ghostBuildingRenderer.material.color = new Color(1f, 1f, 1f, 0.5f);
            _ghostBuildingManager = _ghostBuilding.GetComponentInChildren<GhostBuildingManager>();
        } else if (_isPlacing && Input.GetKeyDown(KeyCode.Escape))
        {
            CleanupGhost();
        }

        if (_isPlacing && Input.GetMouseButtonDown((int) MouseButton.LeftMouse))
        {
            PlaceBuilding();
        }
        
        if (_isPlacing)
        {
            UpdateGhost();    
        }

        
    }

    private void CleanupGhost()
    {
        _isPlacing = false;
        _ghostBuildingRenderer = null;
        Destroy(_ghostBuilding);
    }

    private void PlaceBuilding()
    {
        if (!_ghostBuildingManager.canBePlaced)
        {
            return;
        }

        Instantiate(buildingPrefab, _ghostBuilding.transform.position, _ghostBuilding.transform.rotation);
        CleanupGhost();
    }

    private void UpdateGhost()
    {
        Ray mouseToWorldRay = _camera.ScreenPointToRay(Input.mousePosition);
        //Shoots a ray into the 3D world starting at our mouseposition
        if (Physics.Raycast(mouseToWorldRay, out var hitInfo))
        {
            if (hitInfo.collider.CompareTag("Ground"))
            {
                _ghostBuilding.transform.position = hitInfo.point;    
            }
        }

    }
}
