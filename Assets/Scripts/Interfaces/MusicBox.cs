using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.Playables;

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
    [SerializeField] PlayableDirector _atticTimeline;

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
            //_yokai.SetActive(true);
            StartCoroutine(BackToPlayerCam());
        }
        else
        {
            return;
        }

        
    }

    private void _atticTimeline_stopped(PlayableDirector obj)
    {
      
    }

    IEnumerator BackToPlayerCam()
    {
        yield return new WaitForSeconds(2);
        _playerCam.gameObject.SetActive(true);
        _sceneCam.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        _atticTimeline.Play();
        Gamepad.current.SetMotorSpeeds(0.25f, 0.75f);
        _mirror.SetActive(false);
        _brokenMirror.SetActive(true);
        _yokai.SetActive(true);
        _atticTimeline.stopped += _atticTimeline_stopped;
        _yokai.SetActive(true);

        yield break;
    }


}
