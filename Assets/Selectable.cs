using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;
using UnityEngine.AI;

public class Selectable : MonoBehaviour
{

    public string name;

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
            
        }
    }

    private NavMeshAgent _agent;
    private Outline _outline;
    private Animator _animator;
    private static readonly int Running = Animator.StringToHash("running");
    private bool _isAnimatorNotNull;

    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator> ();
        _isAnimatorNotNull = _animator != null;
        _outline = GetComponentInChildren<Outline>();
        if (_outline)
        {
            _outline.enabled = false;
            _outline.color = 1; // Green
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_isAnimatorNotNull)
        {
            AnimationUpdate();
        }
    }

    private void AnimationUpdate()
    {
        _animator.SetBool(Running, _agent.velocity.magnitude > 0f);
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
