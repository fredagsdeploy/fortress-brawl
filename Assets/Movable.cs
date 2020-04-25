using cakeslice;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Movable : MonoBehaviour, ISelectable, IMovable
{
    public string name;
    
    private NavMeshAgent _agent;
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
        _clonedGoalPost = Instantiate(goalPost, transform.position, Quaternion.identity);
        _goalpostRenderer = _clonedGoalPost.GetComponent<Renderer>();
        _goalpostRenderer.enabled = false;
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

    public void SelectionChanged(bool value)
    {
        _goalpostRenderer.enabled = value;
    }
}