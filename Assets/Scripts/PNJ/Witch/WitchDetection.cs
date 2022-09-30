using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchDetection : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Transform _player;
    [SerializeField] GameObject _witch;
    [SerializeField] float  _speed;
    [SerializeField] float _speedChase;
    bool _seePlayer = false;
    bool _playerLost = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMouvement>())
        {
            Debug.Log("joueur entré");
            _seePlayer = true;
            _playerLost = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMouvement>())
        {
            Debug.Log("Joueur sorti");
            _seePlayer = false;
            StartCoroutine("SawPlayer");
        }
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.red);

        //Rotation
        _witch.transform.LookAt(_player);
        _witch.transform.Rotate(0, 180, 0);

        //If witch sees player
        if (_playerLost == false)
        {                                                                                               
            _animator.SetBool("isRunning", true);
            Vector3 direction = _witch.transform.position - _player.transform.position;
            direction = direction.normalized;
            _witch.transform.position -= direction * Time.deltaTime * _speed;

            if (_seePlayer == true)
            {
                //Witch towards player
                Vector3 directionChase = _witch.transform.position - _player.transform.position;
                directionChase = directionChase.normalized;
                _witch.transform.position -= directionChase * Time.deltaTime * _speedChase;
            }
            else
            {
                _animator.SetBool("isRunning", false);
            }
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }
    }


    IEnumerator SawPlayer()
    {
        yield return new WaitForSeconds(5);
        _playerLost = true;
        Debug.Log("joueur perdu");
        yield break;
    }
}
