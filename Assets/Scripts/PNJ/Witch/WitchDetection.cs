using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WitchDetection : MonoBehaviour
{
    enum WitchState { IDLE, CHASE, SEARCH, PATROL }

    [SerializeField] Animator _animator;
    [SerializeField] GameObject _witch;
    [SerializeField] Rigidbody _witchRb;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Transform[] _points;
    [SerializeField] float  _speed;
    [SerializeField] float _speedChase;
    int _currentPoint;
    Vector3 _targetDirection;
    float _remainingDistance;
    WitchState _state;
    PlayerMouvement _playerDetected;

    private void Awake()
    {
        _state = WitchState.PATROL;
    }

    private void Start()
    {
        _currentPoint = 0;
        _agent = FindObjectOfType<NavMeshAgent>();
        UpdateDestination();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerMouvement>(out var m))
        {
            _state = WitchState.CHASE;
            _playerDetected = m;
            Debug.Log("joueur entré");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerMouvement>(out var m))
        {
            _playerDetected = m;
            _state = WitchState.CHASE;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMouvement>())
        {
            _state = WitchState.SEARCH;
            Debug.Log("Joueur sorti");
            StartCoroutine(SawPlayer());
        }
    }

    IEnumerator SawPlayer()
    {
        yield return new WaitForSeconds(5);
        //_state = WitchState.IDLE;
        _playerDetected = null;
        Debug.Log("joueur perdu");
        _state = WitchState.PATROL;
        yield break;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(_witchRb.position, _witchRb.transform.forward * 5, Color.red);

        //Rotation
        if(_playerDetected !=null)
        {
            _witchRb.transform.LookAt(_playerDetected.transform.position);
        }
        
        Vector3 futurPosition = _witchRb.transform.position;
        switch (_state)
        {
            case WitchState.IDLE:
                // NO move
                _animator.SetBool("isRunning", false);
                _animator.SetBool("isWalking", false);
                break;
            case WitchState.CHASE:
                Debug.Log("1");
                _animator.SetBool("isRunning", true);
                _animator.SetBool("isWalking", false);
                //Witch towards player
                //Vector3 directionChase = (_playerDetected.transform.position - _witchRb.transform.position).normalized;
                //futurPosition += directionChase * Time.fixedDeltaTime * _speedChase;
                _agent.SetDestination(_playerDetected.transform.position);
                break;
            case WitchState.SEARCH:
                Debug.Log("2");
                _animator.SetBool("isRunning", false);
                _animator.SetBool("isWalking", true);
                //Vector3 direction = (_playerDetected.transform.position - _witchRb.transform.position).normalized;
                //futurPosition += direction * Time.fixedDeltaTime * _speed;
                _agent.SetDestination(_playerDetected.transform.position);
                break;
            case WitchState.PATROL:
                _animator.SetBool("isRunning", false);
                _animator.SetBool("isWalking", true);
                Debug.Log("3");
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
                break;
            default:
                break;
        }

        //_witchRb.MovePosition(futurPosition);
    }

    private void UpdateDestination()
    {
        _targetDirection = _points[_currentPoint].position;
        _agent.SetDestination(_targetDirection);
    }

    private void IncremanteCurrentPoint()
    {
        _currentPoint++;
        if (_currentPoint == _points.Length)
        {
            _currentPoint = 0;
        }
    }

}
