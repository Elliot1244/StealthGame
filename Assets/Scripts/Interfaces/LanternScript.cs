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

    private void Start()
    {
        _playerLantern.SetActive(false);
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
