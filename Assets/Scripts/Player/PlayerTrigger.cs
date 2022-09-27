using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] Material[] _tvMats;
    [SerializeField] Material _tvMat1;
    [SerializeField] Material _tvMat2;
    [SerializeField] GameObject _tv1;

    // Start is called before the first frame update
    void Start()
    {


        /*Renderer _tv = _tv1.GetComponent<Renderer>();

        _tvMats[0] = _tvMat1;
        _tvMats[1] = _tvMat2;

        _tv.materials = _tvMats;*/
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<TvScript>() != null)
        {
            Renderer _tv = _tv1.GetComponent<Renderer>();

            _tvMats[0] = _tvMat1;
            _tvMats[1] = _tvMat2;

            _tv.sharedMaterials = _tvMats;
            //Debug.Log("Test Tv");
        }
    }
}
