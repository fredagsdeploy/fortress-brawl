using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace Code
{
    public class WorkerUnitManager : MonoBehaviour
    {
        public bool derp = false;
        private Queue<WorkerUnitTask> _tasks = new Queue<WorkerUnitTask>();
        [CanBeNull] private WorkerUnitTask _currentTask;
        private float _workingRange = 10f;
        private bool _inRange = false;
        private IMovable _movable;
        private Animator _animator;
        private static readonly int Working = Animator.StringToHash("working");

        void Start()
        {
            _movable = GetComponentInChildren<IMovable>();
            _animator = GetComponentInChildren<Animator>();
        }

        public void AddTask(WorkerUnitTask task)
        {
            Debug.Log("Adding task " + task);
            _tasks.Enqueue(task);
            Debug.Log($"Task added {GetInstanceID()} {task} {_tasks.Count}");
            derp = true;
        }

        public void ReplaceQueueWithTask(WorkerUnitTask task)
        {
            ClearTasks();
            AddTask(task);
        }

        public void ClearTasks()
        {
            Debug.Log("Clear tasks");
            foreach (var t in _tasks)
            {
                DestroyTask(t);
            }

            _tasks.Clear();
            if (_currentTask != null)
            {
                DestroyTask(_currentTask);
            }

            _currentTask = null;
        }

        private static void DestroyTask(WorkerUnitTask t)
        {
            if (t.target.State == BuildingInfo.BuildingState.Ghost)
            {
                Destroy(t.target.gameObject);
            }
        }

        void Update()
        {
            Debug.Log($"Tasks count {GetInstanceID()} {_tasks.Count}");
            if (_currentTask != null && _currentTask.IsComplete())
            {
                Debug.Log("Task done");
                _animator.SetBool(Working, false);
                _currentTask = null;
            }

            if (_currentTask == null && _tasks.Any())
            {
                Debug.Log("Start new task");
                StartNextTask();
            }

            PerformWork();
        }

        private void StartNextTask()
        {
            _currentTask = _tasks.Dequeue();
            _movable.SetDestination(_currentTask.target.transform.position);
            _inRange = false;
            Debug.Log("StartNextTask " + _currentTask.target);
        }

        private void PerformWork()
        {
            if (_currentTask == null)
            {
                return;
            }

            Debug.Log("PerformWork");
            if (_currentTask.target == null)
            {
                _movable.Stop();
                _animator.SetBool(Working, false);
                _currentTask = null;
                Debug.Log("PerformWork Target is null");
                return;
            }

            Debug.Log("PerformWork working");
            switch (_currentTask.taskType)
            {
                case WorkerUnitTask.TaskType.Construct:
                    PerformBuild();
                    break;
                case WorkerUnitTask.TaskType.Repair:
                    PerformRepair();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PerformRepair()
        {
            if (!_inRange) return;
        
            _animator.SetBool(Working, true);
            var repairRate = 0.10f;
            _currentTask.target.Progress += repairRate * Time.deltaTime;
        }

        private void PerformBuild()
        {
            if (!(Vector3.Distance(_currentTask.target.transform.position, transform.position) < 12f))
            {
                return;
            }

            _animator.SetBool(Working, true);
            if (_currentTask.target.State == BuildingInfo.BuildingState.Ghost)
            {
                _currentTask.target.State = BuildingInfo.BuildingState.Construction;
            }
            _currentTask.target.Progress += (1f /_currentTask.target.buildTimeInSeconds) * Time.deltaTime;
        }


        private void OnTriggerStay(Collider other)
        {
            if (_currentTask == null || other.CompareTag("Ground"))
            {
                return;
            }


            var buildingInfo = other.gameObject.GetComponentInParent<BuildingInfo>();
            if (buildingInfo == _currentTask.target)
            {
                _inRange = true;
                _movable.Stop();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_currentTask == null || other.CompareTag("Ground"))
            {
                return;
            }

            var buildingInfo = other.gameObject.GetComponentInChildren<BuildingInfo>();
            if (buildingInfo == _currentTask.target)
            {
                _inRange = false;
            }
        }
    }

    public class WorkerUnitTask
    {
        public enum TaskType
        {
            Construct,
            Repair
        }

        public BuildingInfo target;
        public TaskType taskType;

        public static WorkerUnitTask Create(BuildingInfo buildingInfo)
        {
            TaskType taskType = TaskType.Construct;
            switch (buildingInfo.State)
            {
                case BuildingInfo.BuildingState.Ghost:
                    break;
                case BuildingInfo.BuildingState.Construction:
                    taskType = TaskType.Construct;
                    break;
                case BuildingInfo.BuildingState.Completed:
                    taskType = TaskType.Repair;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new WorkerUnitTask()
            {
                target = buildingInfo,
                taskType = taskType
            };
        }

        public bool IsComplete()
        {
            switch (taskType)
            {
                case TaskType.Construct:
                    return target.IsComplete;
                case TaskType.Repair:
                    return target.HasMaxHealth;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return $"{nameof(target)}: {target}, {nameof(taskType)}: {taskType}";
        }
    }
}