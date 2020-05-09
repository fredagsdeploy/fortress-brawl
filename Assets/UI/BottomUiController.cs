using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code;
using JetBrains.Annotations;
using Races;
using UI;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Selectable = Code.Selectable;

public class BottomUiController : MonoBehaviour
{

    public ActionButtonsController actionButtonsController;
    public Text unitSelectionText;
    public ValueBar unitHealthBar;
    public ValueBar unitManaBar;
    public GameObject unitPortrait;

    [CanBeNull] private Camera _singleSelectionCamera;
    [CanBeNull] private GameObject _singleSelection;
    [CanBeNull] private EntityInfo _selectionEntityInfo;
    

    private void Awake()
    {
        unitSelectionText.enabled = false;
    }

    private void Update()
    {
        UpdateDynamicUnitInfo();
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
        actionButtonsController.ClearSelection();
    }

    private void UpdateUnitInfo(List<Selectable> selected)
    {
        unitSelectionText.enabled = true;
        unitSelectionText.text = selected.Count().ToString();
        actionButtonsController.ClearSelection();
    }

    private void UpdateUnitInfo(Selectable selected)
    {
        actionButtonsController.ClearSelection();
        unitSelectionText.enabled = true;
        unitPortrait.SetActive(true);
        _selectionEntityInfo = selected.GetComponentInParent<EntityInfo>();
        unitSelectionText.text = _selectionEntityInfo.entityName;
        _singleSelection = selected.gameObject;
        _singleSelectionCamera = _singleSelection.GetComponentInChildren<Camera>();
        UpdateDynamicUnitInfo();

        var workerUnitManager = _singleSelection.GetComponent<WorkerUnitManager>();
        Debug.Log($"Selected {selected}");
        if (workerUnitManager)
        {
            actionButtonsController.ConstructionUnitSelected(_selectionEntityInfo.race);
        }
        
        if (_singleSelectionCamera)
        {
            _singleSelectionCamera.enabled = true;    
        }
    }

    private void UpdateDynamicUnitInfo()
    {
        if (!_selectionEntityInfo) return;
        
        unitHealthBar.SetValue(_selectionEntityInfo.health, _selectionEntityInfo.maxHealth);
        unitManaBar.SetValue(_selectionEntityInfo.mana, _selectionEntityInfo.maxMana);

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
