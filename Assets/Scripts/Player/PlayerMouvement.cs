using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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
    [SerializeField] GameObject _lanternLight;
    [SerializeField] Canvas _canva;
    [SerializeField] Image _inputImage;
    [SerializeField] Slider _lightSlider;
    [SerializeField] float  _light;
    [SerializeField] float _maxLight;
    [SerializeField] int _isWalkingAnim;
    [SerializeField] int _isWalkingWithlantern;
    [SerializeField] float _speed;
    [SerializeField] float _gravity;
    [SerializeField] int  _stockPile;
    [SerializeField] int _RefuelLantern;
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
    bool _isRefuling;

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

    internal void HavePile()
    {
        _stockPile++;
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
                _lightSlider.gameObject.SetActive(true);
                _useLantern = true;
                StartCoroutine(LightDecrease());
            }
            else
            {
                StopCoroutine(LightDecrease());
                _animator.SetTrigger("unusedLantern");
                _playerLantern.SetActive(false);
                _useLantern = false;
                _lightSlider.gameObject.SetActive(false);
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
        _RefuelLantern = Animator.StringToHash("RefuelLantern");

        
    }

    void Update()
    {
        _animator.SetBool("isRunning", _isRunning);
        Movement();

        //Lantern Management
        if (_useLantern == true)
        {
            if (_lightSlider.value <= 0 && _stockPile > 0 && !_isRefuling)
            {
                StartCoroutine(RefuelRoutine());
                IEnumerator RefuelRoutine()
                {
                    _isRefuling = true;
                    _lightSlider.gameObject.SetActive(false);
                    _lanternLight.gameObject.SetActive(false);

                    //Animation
                    _animator.SetBool(_RefuelLantern, true);
                    yield return new WaitForSeconds(1f);

                    _lightSlider.value = _maxLight;
                    _lightSlider.gameObject.SetActive(true);
                    _lanternLight.gameObject.SetActive(true);
                    _stockPile--;
                    Debug.Log(_lightSlider.value);
                    _isRefuling = false;
                    _light = 100;
                    StartCoroutine(LightDecrease());
                }
            }
            else if(_lightSlider.value <= 0 && _stockPile <= 0)
            {
                _lightSlider.gameObject.SetActive(false);
                _lanternLight.gameObject.SetActive(false);
            }
        } 
    }

    IEnumerator LightDecrease()
    {
        while (_lightSlider.value > 0)
        {
            _light -= 5f;
            _lightSlider.value = _light;
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
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
