using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMouvement : MonoBehaviour
{
    [SerializeField] float _waitBeforeFalling = 15f;
    [SerializeField] InputActionReference _movement;
    [SerializeField] InputActionReference _sprint;
    [SerializeField] InputActionReference _action;
    [SerializeField] InputActionReference _useObject;
    [SerializeField] Animator _animator;
    [SerializeField] CharacterController _controller;
    [SerializeField] Camera _camera;
    [SerializeField] AnimationCurve _climbY;
    [SerializeField] AnimationCurve _climbZ;
    [SerializeField] GameObject _playerLantern;
    [SerializeField] int _isWalkingAnim;
    [SerializeField] int _isWalkingWithlantern;
    [SerializeField] float _speed;
    [SerializeField] float _gravity;
    private float _vSpeed = 0;

    Vector3 _currentMovement;
    bool _movementPressed;
    bool _isRunning;
    bool _lanternPicked;
    bool _isPicking;
    bool _useLantern;
    //bool _lanternActive = false;

    bool _climbing;
    bool _isOpeningDoor;

    private void Reset()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    internal void Climb()
    {
        if (_climbing) return;

        _climbing = true;
        _animator.SetTrigger("canClimb");
        
        StartCoroutine(ClimbMovement());
        IEnumerator ClimbMovement()
        {
            var startPosition = _controller.transform.position;
            float timer = 0;
            while (timer < _climbY.keys[_climbY.keys.Length-1].time)
            {
                yield return null;
                timer += Time.deltaTime;

                _controller.transform.position = startPosition + new Vector3(0, _climbY.Evaluate(timer), _climbZ.Evaluate(timer)); 
            }
            new WaitForSeconds(_waitBeforeFalling);
            _animator.SetTrigger("isFalling");
        }
    }

    internal void ClimbStop()
    {
        _climbing = false;
    }

    internal void OpenDoor()
    {
        if(_isOpeningDoor) return;

        _isOpeningDoor = true;
        _animator.SetTrigger("canOpenDoor");
    }

    internal void PickUp()
    {
        if(_isPicking) return;

        _isPicking = true;
        _animator.SetTrigger("isPickingUp");
        _lanternPicked = true;
    }

    internal void UseLantern()
    {
        _useLantern = true;
    }

    internal void OpenDoorStop()
    {
        _isOpeningDoor = false;
    }

    private void Awake()
    {
        //Mouvement
        _movement.action.performed += StartMovement;
        _movement.action.canceled += StopMovement;

        //Sprint
        _sprint.action.started += SprintStarted;
        _sprint.action.performed += SprintUpdate;
        _sprint.action.canceled += SprintCanceled;

        //Object
        _useObject.action.performed += UseObject;
    }



    private void SprintCanceled(InputAction.CallbackContext obj)
    {
        _isRunning = false;
    }

    private void SprintUpdate(InputAction.CallbackContext obj)
    {
        _isRunning = true;
    }

    private void SprintStarted(InputAction.CallbackContext obj)
    {
        _isRunning = true;
    }

    private void UseObject(InputAction.CallbackContext obj)
    {
        if (_lanternPicked == true)
        {
            if (_useLantern == false)
            {
                _animator.SetTrigger("useLantern");
                _playerLantern.SetActive(true);
                Debug.Log("Use lantern");
                _useLantern = true;
            }
            else
            {
                _animator.SetTrigger("unusedLantern");
                _playerLantern.SetActive(false);
                _useLantern = false;
                Debug.Log("Unused Lantern");
            }
        }
        else
        {
            Debug.Log("No object to use");
        }
    }

    void Start()
    {
        _isWalkingAnim = Animator.StringToHash("isWalking");
        _isWalkingWithlantern = Animator.StringToHash("WalkWithLantern");
    }


    void Update()
    {
        _animator.SetBool("isRunning", _isRunning);
        Movement();
    }

    private void StartMovement(InputAction.CallbackContext obj)
    {
        _currentMovement = obj.ReadValue<Vector2>();
        _movementPressed = true;
    }

    private void StopMovement(InputAction.CallbackContext obj)
    {
        _currentMovement = Vector2.zero;
        _movementPressed = false;
    }

    void Movement()
    {
        bool _isWalking = _animator.GetBool(_isWalkingAnim);
        if (_movementPressed)
        {
            _animator.SetBool(_isWalkingAnim, true);
        }
        else
        {
            _animator.SetBool(_isWalkingAnim, false);
        }


        //If player move with lantern
        if(_movementPressed && _useLantern == true)
        {
            _animator.SetBool(_isWalkingWithlantern, true);
        }
        else
        {
            _animator.SetBool(_isWalkingWithlantern, false);
        }

        // Camera
        var realDirection = new Vector3(_currentMovement.x, 0, _currentMovement.y);
        realDirection = _camera.transform.TransformDirection(realDirection);
        if(!_climbing || _isOpeningDoor)
        {
            // Rotation
            _controller.transform.LookAt(_controller.transform.position + realDirection);
            _controller.transform.rotation = Quaternion.Euler(0, _controller.transform.rotation.eulerAngles.y, 0);
        }
       
        // Gravity
        _vSpeed -= _gravity * Time.deltaTime;
        realDirection.y = _vSpeed;

        if(!_climbing || !_isOpeningDoor)
        {
            if (_isRunning)

            {
                _controller.Move(realDirection * _speed * Time.deltaTime * 2);
                _isRunning = true;
            }
            else
            {
                _controller.Move(realDirection * _speed * Time.deltaTime);
                _isRunning = false;
            }
        }
    }

}
