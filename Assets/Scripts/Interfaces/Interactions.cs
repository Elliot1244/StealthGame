using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Interactions : MonoBehaviour
{
    [SerializeField] Canvas _canva;
    [SerializeField] InputActionReference _action;
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

    // Start is called before the first frame update
    void Start()
    {
        _canva.gameObject.SetActive(false);
    }

    private void ActionStarted(InputAction.CallbackContext obj)
    {
        _actionButtonPressd = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.yellow);
        if (Physics.Raycast(transform.position, transform.forward, out var hit, 2f))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactableObject))
            {
                _canva.gameObject.SetActive(true);
                if(hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable) && _actionButtonPressd == true)
                {
                    interactable.Use();
                }
            }
            else
            {
                _canva.gameObject.SetActive(false);
            }
        }
        else
        {
            _canva.gameObject.SetActive(false);
        }
    }
}
