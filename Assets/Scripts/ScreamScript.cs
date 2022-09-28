using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamScript : MonoBehaviour
{
    [SerializeField] GameObject _scream;
    // Start is called before the first frame update
    void Start()
    {
        _scream.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMouvement>() != null)
        {
           _scream.SetActive(true);
        }
    }
}
