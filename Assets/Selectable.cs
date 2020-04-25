using System;
using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    private Outline _outline;
    private bool _isSelected;

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            if (_outline)
            {
                _outline.enabled = value;
            }
            SendMessage(nameof(ISelectable.SelectionChanged), value, SendMessageOptions.DontRequireReceiver);
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
            _outline.enabled = false;
            _outline.color = 1; // Green
        }
    }
    
    void OnEnable()
    {
        SelectionManager.selectables.Add(this);
    }

    void OnDisable()
    {
        SelectionManager.selectables.Remove(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
