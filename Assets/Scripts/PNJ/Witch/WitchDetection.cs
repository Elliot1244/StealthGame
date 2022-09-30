using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchDetection : MonoBehaviour
{
    enum WitchState { IDLE, CHASE, SEARCH }

    [SerializeField] Animator _animator;
    [SerializeField] GameObject _witch;
    [SerializeField] Rigidbody _witchRb;
    [SerializeField] float  _speed;
    [SerializeField] float _speedChase;

    WitchState _state;
    PlayerMouvement _playerDetected;

    private void Awake()
    {
        _state = WitchState.IDLE;
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
            //Debug.Log("joueur entré");
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
            StartCoroutine("SawPlayer");
        }
    }

    IEnumerator SawPlayer()
    {
        yield return new WaitForSeconds(5);
        _state = WitchState.IDLE;
        _playerDetected = null;
        Debug.Log("joueur perdu");
        yield break;
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
                break;
            case WitchState.CHASE:
                Debug.Log("1");
                _animator.SetBool("isRunning", true);
                //Witch towards player
                Vector3 directionChase = (_playerDetected.transform.position - _witchRb.transform.position).normalized;
                futurPosition += directionChase * Time.fixedDeltaTime * _speedChase;
                break;
            case WitchState.SEARCH:
                Debug.Log("2");
                _animator.SetBool("isRunning", false);
                Vector3 direction = (_playerDetected.transform.position - _witchRb.transform.position).normalized;
                futurPosition += direction * Time.fixedDeltaTime * _speed;
                break;
            default:
                break;
        }

        _witchRb.MovePosition(futurPosition);
    }


    
}
