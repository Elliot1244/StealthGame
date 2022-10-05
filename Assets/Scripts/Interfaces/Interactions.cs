using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class Interactions : MonoBehaviour
{
    [SerializeField] Canvas _canva;
    [SerializeField] Image _lightRemaining;
    [SerializeField] InputActionReference _action;
    [SerializeField] PlayerMouvement _movement;
    [SerializeField] Image _inputImage;
    bool _actionButtonPressd = false;

    private void Awake()
    {
        //Action
        _action.action.started += ActionStarted;
        _action.action.canceled += ActionCanceled;
        
    }

    private void ActionCanceled(InputAction.CallbackContext obj)
    {
        _actionButtonPressd = false;
    }

    internal void WaitClimbAnimationEnd()
    {
        _movement.Climb();
    }

    internal void WaitEnterDoorAnimationEnd()
    {
        _movement.OpenDoor();
    }

    internal void WaitPickUpAnimationEnd()
    {
        _movement.PickUp();
    }

    internal void UseLanter()
    {
        _movement.UseLantern();
    }

    // Start is called before the first frame update
    void Start()
    {
        _canva.gameObject.SetActive(true);
    }

    private void ActionStarted(InputAction.CallbackContext obj)
    {
        _actionButtonPressd = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 1, Color.yellow);
        if (Physics.Raycast(transform.position, transform.forward, out var hit, 1f))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactableObject))
            {
                if(interactableObject.IsInteractable)
                {
                    //_canva.gameObject.SetActive(true);
                    _inputImage.gameObject.SetActive(true);
                    //_lightRemaining.gameObject.SetActive(false);
                    if (_actionButtonPressd == true)
                    {
                        interactableObject.Use(this);
                    }
                }
            }
            else
            {
                _inputImage.gameObject.SetActive(false);
                //_canva.gameObject.SetActive(true);
                //_lightRemaining.gameObject.SetActive(false);
            }
        }
        else
        {
            //_canva.gameObject.SetActive(false);
            _inputImage.gameObject.SetActive(false);
        }

        //_lightRemaining.gameObject.SetActive(false);
        //_inputImage.gameObject.SetActive(false);
    }
}
