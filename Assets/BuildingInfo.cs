using System;
using UnityEngine;
using Utils;

public class BuildingInfo : EntityInfo
{
    public enum BuildingState
    {
        Ghost,
        Construction,
        Completed
    }

    public GameObject ghostPrefab;
    public GameObject constructionPrefab;
    public GameObject completedPrefab;
    public float buildTimeInSeconds;
    private GameObject _model;
    private float _progress;
    private BuildingState _state;
    
    public BuildingState State
    {
        get => _state;
        set
        {
            _state = value;
            
            var _oldModel = _model;
            switch (_state)
            {
                case BuildingState.Ghost:
                    _model = Instantiate(ghostPrefab, transform);
                    break;
                case BuildingState.Construction:
                    if (_oldModel.GetComponentInChildren<GhostBuildingManager>().canBePlaced)
                    {
                        _model = Instantiate(constructionPrefab, transform);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                    break;
                case BuildingState.Completed:
                    _model = Instantiate(completedPrefab, transform);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
                
            }
            if (_oldModel)
            {
                Destroy(_oldModel);
            }
        }
    }
    
    public float Progress
    {
        get => _progress;
        set
        {
            _progress = Mathf.Clamp(value, 0, 1f);
            if (_state == BuildingState.Construction)
            {
                var newTransform = new Vector3(1, _progress, 1);
                _model.transform.localScale = newTransform;
            }
        }
    }

    public bool IsComplete => Math.Abs(_progress - 1f) < 0.1f;
    
    private void Update()
    {
        if(IsComplete)
        {
            switch (State)
            {
                case BuildingState.Ghost:
                    State = BuildingState.Construction;
                    Progress = 0;
                    break;
                case BuildingState.Construction:
                    State = BuildingState.Completed;
                    health = maxHealth;
                    break;
                case BuildingState.Completed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
    }
}
