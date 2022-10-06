using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileScript : MonoBehaviour, IInteractable
{
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _pile;
    [SerializeField] int _pickObjectAnim;
    public bool IsInteractable => true;

    private void Start()
    {
        _pickObjectAnim = Animator.StringToHash("PickObject");
    }
    public string GetName()
    {
        return "";
    }

    public void Use(Interactions master)
    {
        _animator.SetTrigger("PickObject");
        Destroy(_pile);
        Debug.Log("Pile taken");
        master.HavePile();
    }
}
