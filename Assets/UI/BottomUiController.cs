using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class BottomUiController : MonoBehaviour
{
    public Text unitSelectionText;
    public GameObject unitPortrait;
    [CanBeNull] private Camera _singleSelectionCamera;
    [CanBeNull] private GameObject _singleSelection;

    private void Awake()
    {
        unitSelectionText.enabled = false;
    }

    public void OnUpdatedSelection(List<Selectable> selected)
    {
        if (_singleSelectionCamera)
        {
            _singleSelectionCamera.enabled = false;
            _singleSelectionCamera = null;
        }
        if (selected.Count() == 1)
        {
            UpdateUnitInfo(selected.First());
        }
        else if(selected.Any())
        {
            UpdateUnitInfo(selected);
        }
        else
        {
            ClearSelectionUpdate();
        }
    }

    private void ClearSelectionUpdate()
    {
        unitSelectionText.enabled = false;
        unitPortrait.SetActive(false);
    }

    private void UpdateUnitInfo(List<Selectable> selected)
    {
        unitSelectionText.enabled = true;
        unitSelectionText.text = selected.Count().ToString();
    }

    private void UpdateUnitInfo(Selectable selected)
    {
        unitSelectionText.enabled = true;
        unitPortrait.SetActive(true);
        unitSelectionText.text = selected.entityName;
        _singleSelection = selected.gameObject;
        _singleSelectionCamera = _singleSelection.GetComponentInChildren<Camera>();
        
        if (_singleSelectionCamera)
        {
            _singleSelectionCamera.enabled = true;    
        }
    }

    public void PortraitClicked()
    {
        if (_singleSelection)
        {
            var destination = _singleSelection.transform.position;
            destination.y = Camera.main.transform.position.y;
            destination.z -= destination.y;
            Camera.main.transform.position = destination;
        }
    }
}
