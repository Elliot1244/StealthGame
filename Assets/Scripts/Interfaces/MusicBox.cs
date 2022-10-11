using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.InputSystem;

public class MusicBox : MonoBehaviour, IInteractable
{
    [SerializeField] Animator _handleAnimator;
    [SerializeField] Animator _doolAnimator;  
    [SerializeField] AudioSource _boxMusic;
    [SerializeField] Camera _playerCam;
    [SerializeField] Camera _sceneCam;
    [SerializeField] GameObject _mirror;
    [SerializeField] GameObject _brokenMirror;
    [SerializeField] GameObject _yokai;

    bool _isInteracted = false;
    int _activeHandle;
    int _activeDool;


    //Vibration
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    private void Start()
    {
        _activeHandle = Animator.StringToHash("isInactive");
        _activeDool = Animator.StringToHash("isInactive");
        _yokai.SetActive(false);
    }

    private void Update()
    {
            GamePad.SetVibration(playerIndex, 0.1f, 0.1f);
            //Handheld.Vibrate();
    }

    public bool IsInteractable => _isInteracted == false;

    public string GetName()
    {
        return "";
    }


    public void Use(Interactions master)
    {
        if (_isInteracted == false)
        {
            _sceneCam.gameObject.SetActive(true);
            _playerCam.gameObject.SetActive(false);
            _handleAnimator.SetTrigger(_activeHandle);
            _doolAnimator.SetTrigger(_activeDool);
            _boxMusic.Stop();
            Debug.Log("Cinématique à jouer");
            master.WaitMusicBoxInteractionEnd();
            _isInteracted = true;
            StartCoroutine(BackToPlayerCam());
            Gamepad.current.SetMotorSpeeds(0.25f, 0.75f);
            _mirror.SetActive(false);
            _brokenMirror.SetActive(true);
            _yokai.SetActive(true);
            


        }
        else
        {
            return;
        }

        
    }

    IEnumerator BackToPlayerCam()
    {
        yield return new WaitForSeconds(2);
        _playerCam.gameObject.SetActive(true);
        _sceneCam.gameObject.SetActive(false);
        yield break;
    }


}
