using cakeslice;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
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
                _goalpostRenderer.enabled = value;
            }
        }
    }

    private NavMeshAgent _agent;
    private Outline _outline;
    private Animator _animator;
    public GameObject goalPost;
    private static readonly int Running = Animator.StringToHash("running");
    private bool _isAnimatorNotNull;
    private Renderer _goalpostRenderer;
    private GameObject _clonedGoalPost;

    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _clonedGoalPost = Instantiate(goalPost, transform.position, Quaternion.identity);
        _goalpostRenderer = _clonedGoalPost.GetComponent<Renderer>();
        _goalpostRenderer.enabled = false;
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
        _clonedGoalPost.transform.position = new Vector3(destination.x, _clonedGoalPost.transform.position.y, destination.z);
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