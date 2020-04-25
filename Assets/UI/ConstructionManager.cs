using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class ConstructionManager : MonoBehaviour
    {
        private bool _isPlacing = false;
        private ConstructionInfo _constructionInfo;

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

        public void StartPlacing(ConstructionInfo constructionInfo)
        {
            Debug.Log("Starting placing " + constructionInfo.ghostBuildingPrefab.name);
            _constructionInfo = constructionInfo;
            _isPlacing = true;
            _ghostBuilding = Instantiate(_constructionInfo.ghostBuildingPrefab, transform, true);
            _ghostBuildingRenderer = _ghostBuilding.GetComponentInChildren<MeshRenderer>();
            _ghostBuildingRenderer.material.color = new Color(1f, 1f, 1f, 0.5f);
            _ghostBuildingManager = _ghostBuilding.GetComponentInChildren<GhostBuildingManager>();
        }

        private void StopPlacing()
        {
            _isPlacing = false;
            _ghostBuildingRenderer = null;
            _constructionInfo = null;
            Destroy(_ghostBuilding);
        }

        private void PlaceBuilding()
        {
            if (!_ghostBuildingManager.canBePlaced)
            {
                return;
            }

            Instantiate(_constructionInfo.buildingPrefab, _ghostBuilding.transform.position, _ghostBuilding.transform.rotation);
            StopPlacing();
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
}