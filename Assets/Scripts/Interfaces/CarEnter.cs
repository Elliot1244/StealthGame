using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEnter : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    public string GetName()
    {
        return "";
    }

    public void Use(Interactions master)
    {
        Debug.Log("There's no point running away from this place");
    }
}
