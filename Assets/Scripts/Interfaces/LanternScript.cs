using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LanternScript : MonoBehaviour, IInteractable
{

    [SerializeField] Animator _animator;
    [SerializeField] GameObject _lantern;
    [SerializeField] GameObject _playerLantern;
    [SerializeField] InputActionReference _useObject;
    [SerializeField] Item _itemConf;

    bool _isPicking;
    bool _lanternPicked;

    private void Start()
    {
        _playerLantern.SetActive(false);
    }

    public bool IsInteractable => true;

    public string GetName()
    {
        return "";
    }

    public void Use(Interactions master)
    {
        master.WaitPickUpAnimationEnd();
        Destroy(_lantern);
        _lanternPicked = true;

        //master.Inventory.Add(_itemConf);
    }
}
