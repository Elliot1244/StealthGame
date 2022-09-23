using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WallEnterScript : MonoBehaviour, IInteractable
{
    [SerializeField] Animator _animator;

    /*private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }*/
    // Start is called before the first frame update
    public string GetName()
    {
        return "";
    }
    public void Use()
    {
        _animator.SetTrigger("canClimb");
        Debug.Log("Je peux faire le mur");
    }
}
