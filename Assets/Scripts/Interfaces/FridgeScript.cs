using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeScript : MonoBehaviour, IInteractable
{
    [SerializeField] Animator _animator;
    [SerializeField] Canvas _canva;
    [SerializeField] GameObject _player;
    [SerializeField] Camera _otherCam;
    [SerializeField] Camera _playerCam;

    bool _isOpen = true;

    public bool IsInteractable => true;

    public string GetName()
    {
        return "";
    }

    public void Use(Interactions master)
    {
        if (_isOpen == true)
        {
            /*_animator.SetTrigger("_isOpen");
            master.WaitEnterDoorAnimationEnd();*/

            _player.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            _player.transform.position = new Vector3(13.037f, 1.06f, 67.049f);
            Debug.Log("Joueur à l'intérieur");
            _otherCam.gameObject.SetActive(true);
            _playerCam.gameObject.SetActive(false);
            _isOpen = false;
            return;
        }
        if(_isOpen == false)
        {
            //_animator.SetTrigger("_isOpen");
            //master.WaitEnterDoorAnimationEnd();

            _player.transform.localScale = new Vector3(1, 1, 1);
            _player.transform.position = new Vector3(13.144f, 1.06f, 66.016f);
            Debug.Log("Joueur dehors");
            _playerCam.gameObject.SetActive(true);
            _otherCam.gameObject.SetActive(false);
            _isOpen = true;
            return;
        }
    }

    /*IEnumerator ChangeCamKitchen()
    {
        yield return new WaitForSeconds(1f);
        
    }*/

    /*IEnumerator BackToPlayerCam()
    {
        yield return new WaitForSeconds(1f);
        
    }*/

}
