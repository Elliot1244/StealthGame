using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        _activeHandle = Animator.StringToHash("isInactive");
        _activeDool = Animator.StringToHash("isInactive");
        _yokai.SetActive(false);


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
            _yokai.SetActive(true);
            /*_mirror.SetActive(false);
            _brokenMirror.SetActive(true);*/

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
