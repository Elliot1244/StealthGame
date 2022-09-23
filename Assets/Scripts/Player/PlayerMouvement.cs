using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMouvement : MonoBehaviour
{
    [SerializeField] InputActionReference _movement;
    [SerializeField] InputActionReference _sprint;
    [SerializeField] InputActionReference _action;
    [SerializeField] Animator _animator;
    [SerializeField] CharacterController _controller;
    [SerializeField] Camera _camera;
    [SerializeField] int _isWalkingAnim;
    [SerializeField] float _speed;
    [SerializeField] float _gravity;
    //[SerializeField] Transform _root;
    private float _vSpeed = 0;

    Vector3 _currentMovement;
    bool _movementPressed;
    bool _isRunning;

    bool _climbing;

    private void Reset()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    [SerializeField] AnimationCurve _climbY;
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

                _controller.transform.position = startPosition + new Vector3(0, _climbY.Evaluate(timer), 0);
            }
        }
    }

    internal void ClimbStop()
    {
        _climbing = false;
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

    void Start()
    {
        _isWalkingAnim = Animator.StringToHash("isWalking");
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

        // Camera
        var realDirection = new Vector3(_currentMovement.x, 0, _currentMovement.y);
        realDirection = _camera.transform.TransformDirection(realDirection);

        // Rotation
        _controller.transform.LookAt(_controller.transform.position + realDirection);
        _controller.transform.rotation = Quaternion.Euler(0, _controller.transform.rotation.eulerAngles.y, 0);


        // Gravity
        _vSpeed -= _gravity * Time.deltaTime;
        realDirection.y = _vSpeed;

        if(!_climbing)
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
