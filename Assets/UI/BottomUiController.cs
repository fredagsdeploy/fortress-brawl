﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UI;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class BottomUiController : MonoBehaviour
{

    public ActionButtonsController actionButtonsController;
    public Text unitSelectionText;
    public ValueBar unitHealthBar;
    public ValueBar unitManaBar;
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
        actionButtonsController.ConstructionUnitDeselected();
    }

    private void UpdateUnitInfo(List<Selectable> selected)
    {
        unitSelectionText.enabled = true;
        unitSelectionText.text = selected.Count().ToString();
        actionButtonsController.ConstructionUnitDeselected();
    }

    private void UpdateUnitInfo(Selectable selected)
    {
        unitSelectionText.enabled = true;
        unitPortrait.SetActive(true);
        var entityInfo = selected.GetComponent<EntityInfo>();
        unitSelectionText.text = entityInfo.entityName;
        _singleSelection = selected.gameObject;
        _singleSelectionCamera = _singleSelection.GetComponentInChildren<Camera>();
        unitHealthBar.SetValue(entityInfo.health, entityInfo.maxHealth);
        unitManaBar.SetValue(entityInfo.mana, entityInfo.maxMana);

        if (entityInfo.canConstruct)
        {
            actionButtonsController.ConstructionUnitSelected();
        }
        
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