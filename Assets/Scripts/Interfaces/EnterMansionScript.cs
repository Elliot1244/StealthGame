using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class EnterMansionScript : MonoBehaviour, IInteractable
{
    [SerializeField] Animator _animator;
    [SerializeField] PlayableDirector _doorAnimator;
    [SerializeField] Canvas _canva;
    [SerializeField] UnityEvent _onUse;
    bool _isOpen;

    public bool IsInteractable => _isOpen==false;

    public string GetName()
    {
        return "";
    }

    public void Use(Interactions master)
    {
        if (_isOpen)
        {
            _canva.gameObject.SetActive(false);
            return;
        }
        _doorAnimator.Play();
        //_doorAnimator.stopped += _doorAnimator_stopped;
        //master.WaitEnterDoorAnimationEnd();
        _isOpen = true;
    }

    /*private void _doorAnimator_stopped(PlayableDirector obj)
    {
        throw new System.NotImplementedException();
    }*/
}
