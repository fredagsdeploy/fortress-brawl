using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Code
{
    public class CombatUnitManager : MonoBehaviour
    {

        private CombatInfo _combatInfo;
        private EntityInfo _entityInfo;
        private EntityInfo _currentTarget;
        private List<EntityInfo> _aggroRangeTargets = new List<EntityInfo>();
        private List<EntityInfo> _combatRangeTargets = new List<EntityInfo>();
        private bool _inAttackRange;
        private float _timeSpentOnAttack = -1;
        [CanBeNull] private Movable _movable;
        private Animator _animator;
        private static readonly int Attacking = Animator.StringToHash("attacking");
    
        void Start()
        {
            _entityInfo = GetComponentInParent<EntityInfo>();
            _movable = GetComponentInParent<Movable>();
            _combatInfo = GetComponentInParent<CombatInfo>();
            _animator = _combatInfo.GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            // TODO Update destination (so it doesn't run towards where the target was)
            // TODO Tie animation with attack speed
        
            if (_currentTarget == null && _aggroRangeTargets.Any())
            {
                SetNextTarget();
            }

            CombatAction();
        }

        private void CombatAction()
        {
            if (_entityInfo.IsDead)
            {
                _animator.SetBool(Attacking, false);
                return;
            }
        
            if (_currentTarget && !_inAttackRange && _movable)
            {
                _movable.SetDestination(_currentTarget.gameObject.transform.position);
            }

            if (_inAttackRange)
            {
                if (_timeSpentOnAttack < 0)
                {
                    _timeSpentOnAttack = 0;
                    _animator.SetBool(Attacking, true);
                }
                else
                {
                    _timeSpentOnAttack += Time.deltaTime;
                }

                if (_timeSpentOnAttack > _combatInfo.delayBetweenAttacksInSeconds)
                {
                    _currentTarget.health -= _combatInfo.attackDamagePerAttack;
                
                    // Keep overtime spent, might be more time between frames then time taken for attack
                    _timeSpentOnAttack -= _combatInfo.delayBetweenAttacksInSeconds;
                    if (_currentTarget.IsDead)
                    {
                        _combatRangeTargets.Remove(_currentTarget);
                        _aggroRangeTargets.Remove(_currentTarget);
                        RemoveCurrentTarget();
                    }
                }
            }
        
        
        }

        private void SetNextTarget()
        {
            _currentTarget = _aggroRangeTargets
                .OrderBy(info => Vector3.Distance(info.transform.position, transform.position))
                .First();

            if (_combatRangeTargets.Contains(_currentTarget))
            {
                _inAttackRange = true;
            }
        }


        public void OnCombatRangeEnter(Collider other)
        {
            var entityInfo = GetEntityInfo(other);
            Debug.Log($"CombatRangeEnter: {entityInfo}");
            _combatRangeTargets.Add(entityInfo);
            if (entityInfo == _currentTarget)
            {
                if (_movable)
                {
                    _movable.Stop(); 
                }
                _inAttackRange = true;
            }
        }

        public void OnCombatRangeExit(Collider other)
        {
            var entityInfo = GetEntityInfo(other);
            if (entityInfo == _currentTarget)
            {
                RemoveCurrentTarget();
            }

            _combatRangeTargets.Remove(entityInfo);
        }

        private void RemoveCurrentTarget()
        {
            _currentTarget = null;
            _inAttackRange = false;
            _timeSpentOnAttack = -1;
            _animator.SetBool(Attacking, false);
        }


        public void OnAggroRangeEnter(Collider other)
        {
            var entityInfo = GetEntityInfo(other);
            Debug.Log($"OnAggroRangeEnter: {entityInfo}");
            _aggroRangeTargets.Add(entityInfo);
        }

        public void OnAggroRangeExit(Collider other)
        {
            var entityInfo = GetEntityInfo(other);
            _aggroRangeTargets.Remove(entityInfo);
        }

        private static EntityInfo GetEntityInfo(Collider other)
        {
            return other.GetComponentInParent<EntityInfo>();
        }
    }
}
