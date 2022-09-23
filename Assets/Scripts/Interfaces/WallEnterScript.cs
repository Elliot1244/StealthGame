using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WallEnterScript : MonoBehaviour, IInteractable
{
    [SerializeField] Animator _animator;

    // Start is called before the first frame update
    public string GetName()
    {
        return "";
    }

    public void Use(Interactions master)
    {
        master.WaitClimbAnimationEnd();
        Debug.Log("Je peux faire le mur");
    }

}
