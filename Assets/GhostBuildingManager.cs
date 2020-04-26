using System;
using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class GhostBuildingManager : MonoBehaviour
{

    public bool canBePlaced = true;
    private int _collisions = 0;
    private Outline _outline;
    // Start is called before the first frame update
    void Start()
    {
        _outline = GetComponentInChildren<Outline>();
        if (_outline)
        {
            _outline.color = 2;  
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            return;
        }
        canBePlaced = false;
        _outline.color = 0;
        _collisions++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            return;
        }

        _collisions--;
        if (_collisions == 0)
        {
            canBePlaced = true;
            _outline.color = 2;
        }
    }
}
