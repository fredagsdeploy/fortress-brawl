using System;
using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    private Outline _outline;
    private bool _isSelected = false;

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                if (_outline)
                {
                    _outline.enabled = value;
                }
                SendMessage(nameof(ISelectable.SelectionChanged), value, SendMessageOptions.DontRequireReceiver);
            }

        }
    }

    private void Awake()
    {
        SetupOutline();
    }
    
    private void SetupOutline()
    {
        _outline = GetComponentInChildren<Outline>();
        if (_outline)
        {
            _outline.color = 1; // Green
            _outline.enabled = false;
            
        }
    }
    
    void OnEnable()
    {
        UnitManager.selectables.Add(this);
    }

    void OnDisable()
    {
        UnitManager.selectables.Remove(this);
    }
}
