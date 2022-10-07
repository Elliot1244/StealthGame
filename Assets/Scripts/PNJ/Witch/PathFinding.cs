using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinding : MonoBehaviour
{
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Transform[] _points;
    [SerializeField] Animator _animator;
    [SerializeField] int _speed;
    int _currentPoint;
    Vector3 _targetDirection;

    float _remainingDistance;

    private void Start()
    {
        _currentPoint = 0;
        _agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
        _animator.SetBool("isWalking", true);
    }

    private void Update()
    {
        var agent = _agent.transform.position; 
        agent.y = 0;                             //On ignore la comparaison en y

        var t = _targetDirection;
        t.y = 0;                                //On ignore la comparaison en y


        _remainingDistance = Vector3.Distance(agent, t); //la distance restante (_remainingDistance) est égale à la distance entre la position du navmesh et le point où aller.
        if (_remainingDistance < 1)
        {
            
            IncremanteCurrentPoint();
            UpdateDestination();
        }

    }

    private void UpdateDestination()
    {
        _targetDirection = _points[_currentPoint].position;
        _agent.SetDestination(_targetDirection);
    }

    private void IncremanteCurrentPoint()
    {
        _currentPoint++;
        if(_currentPoint == _points.Length )
        {
            _currentPoint = 0;
        }
    }
}
