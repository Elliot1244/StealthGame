using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LanternScript : MonoBehaviour, IInteractable
{

    [SerializeField] Animator _animator;
    [SerializeField] Canvas _canva;
    [SerializeField] GameObject _lantern;
    [SerializeField] GameObject _playerLantern;
    [SerializeField] InputActionReference _useObject;

    bool _isPicking;
    bool _lanternPicked;
    bool _lanternActive = false;


    private void Awake()
    {
        _useObject.action.performed += UseObject;
    }

    private void Start()
    {
        _playerLantern.SetActive(false);
    }

    private void UseObject(InputAction.CallbackContext obj)
    {
        if (_lanternPicked == true)
        {
            if (_lanternActive == false)
            {
                _animator.SetTrigger("useLantern");
                _playerLantern.SetActive(true);
                Debug.Log("Use lantern");
                _lanternActive = true;
            }
            else
            {
                _animator.SetTrigger("unusedLantern");
            }
        }
        else
        {
            Debug.Log("No object to use");
        }
    }

    public bool IsInteractable => _isPicking == false;

    public string GetName()
    {
        return "";
    }

    public void Use(Interactions master)
    {
        if (_isPicking)
        {
            _canva.gameObject.SetActive(false);
            return;
        }

        master.WaitPickUpAnimationEnd();
        Destroy(_lantern);
        _lanternPicked = true;
    }
}
