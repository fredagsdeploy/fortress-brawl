using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;
using UnityEngine.AI;

public class Selectable : MonoBehaviour
{

    public string name;

    private bool _isSelected = false;
    public bool isSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            _outline.enabled = value;
        }
    }

    private NavMeshAgent _agent;
    private Outline _outline;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
        _outline.color = 1; // Green
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDestination(Vector3 destination)
    {
        _agent.destination = destination;
    }
    
    void OnEnable()
    {
        SelectionManager.selectables.Add(this);
    }
 
    void OnDisable()
    {
        SelectionManager.selectables.Remove(this);
    }
    
}
