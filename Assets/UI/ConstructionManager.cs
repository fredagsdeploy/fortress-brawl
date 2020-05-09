﻿using Code;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace UI
{
    public class ConstructionManager : MonoBehaviour
    {
        private bool _isPlacing = false;
        private BuildingInfo _buildingInfo;

        private GhostBuildingManager _ghostBuildingManager;
        public WorkerUnitManager workerUnitManager;

        private Camera _camera;

        // Start is called before the first frame update
        void Start()
        {
            _camera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            if (_isPlacing && Input.GetKeyDown(KeyCode.Escape))
            {
                StopPlacing();
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

        public void StartPlacing(BuildingInfo buildingInfo)
        {
            Debug.Log("Starting placing " + buildingInfo.ghostPrefab.name);
            _buildingInfo = Instantiate(buildingInfo, DynamicObjectsUtil.DynamicRoot);
            _isPlacing = true;
            
            _buildingInfo.State = BuildingInfo.BuildingState.Ghost;
            _ghostBuildingManager = _buildingInfo.GetComponentInChildren<GhostBuildingManager>();
        }

        public void StopPlacing(bool shouldDestroyGhost = true)
        {
            _isPlacing = false;
            if (shouldDestroyGhost)
            {
                Destroy(_buildingInfo);    
            }
        }

        private void PlaceBuilding()
        {
            if (!_ghostBuildingManager.canBePlaced)
            {
                return;
            }
            
            workerUnitManager.AddTask(WorkerUnitTask.Create(_buildingInfo));
            StopPlacing(false);
        }

        private void UpdateGhost()
        {
            Ray mouseToWorldRay = _camera.ScreenPointToRay(Input.mousePosition);
            //Shoots a ray into the 3D world starting at our mouseposition
            if (Physics.Raycast(mouseToWorldRay, out var hitInfo))
            {
                if (hitInfo.collider.CompareTag("Ground"))
                {
                    _buildingInfo.transform.position = hitInfo.point;
                }
            }
        }
    }
}