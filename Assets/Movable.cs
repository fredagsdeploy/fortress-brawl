using cakeslice;
using UnityEngine;
using UnityEngine.AI;
using Utils;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Movable : MonoBehaviour, ISelectable, IMovable
{
    private NavMeshAgent _agent;
    private Animator _animator;
    public GameObject goalPost;
    private static readonly int Running = Animator.StringToHash("running");
    private bool _isAnimatorNotNull;
    private Renderer _goalpostRenderer;
    private GameObject _clonedGoalPost;
    private bool _moving = false;
    private bool _selected = false;
    private Transform _parent;

    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        SetupAnimator();
        SetupGoalPost();

    }
    
    
    private void SetupAnimator()
    {
        _animator = GetComponentInChildren<Animator>();
        _isAnimatorNotNull = _animator != null;
    }

    private void SetupGoalPost()
    {
        _clonedGoalPost = Instantiate(goalPost, transform.position, Quaternion.identity, DynamicObjectsUtil.DynamicRoot);
        _goalpostRenderer = _clonedGoalPost.GetComponent<Renderer>();
        _goalpostRenderer.enabled = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        _moving = _agent.velocity.magnitude > 0f;
        UpdateGoalPost();
        if (_isAnimatorNotNull)
        {
            AnimationUpdate();
        }
    }

    private void UpdateGoalPost()
    {
        _goalpostRenderer.enabled = _moving && _selected;
    }

    private void AnimationUpdate()
    {
        _animator.SetBool(Running, _moving);
    }

    public void SetDestination(Vector3 destination)
    {
        _clonedGoalPost.transform.position = new Vector3(destination.x, _clonedGoalPost.transform.position.y, destination.z);
        _agent.destination = destination;
        _agent.isStopped = false;
    }

    public void Stop()
    {
        _agent.isStopped = true;
    }

    public void SelectionChanged(bool value)
    {
        _selected = value;
    }
}