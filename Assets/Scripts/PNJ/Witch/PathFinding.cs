using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinding : MonoBehaviour
{
    [SerializeField] NavMeshAgent _agent;
    //[SerializeField] Transform _firstPoint;
    [SerializeField] Transform[] _points;
    [SerializeField] int _speed;
    int _currentPoint;
    Vector3 _targetDirection;

    float _remainingDistance;

    private void Start()
    {
        _currentPoint = 0;
        _agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
    }

    private void Update()
    {
        /*if(_agent.transform.position != _points[_currentPoint].position)
        {
            _agent.SetDestination(_points[_currentPoint].position);
        }
        else
        {
            _currentPoint = (_currentPoint + 1) % _points.Length;
        }*/
        var agent = _agent.transform.position;
        agent.y = 0;
        var t = _targetDirection;
        t.y = 0;
        _remainingDistance = Vector3.Distance(agent, t);
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
