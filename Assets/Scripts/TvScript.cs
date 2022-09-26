using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvScript : MonoBehaviour
{
    [SerializeField] Material[] _tvMats;
    [SerializeField] Material _tvMat1;
    [SerializeField] Material _tvMat2;
    
    // Start is called before the first frame update
    void Start()
    {
        Renderer _tv = GetComponent<Renderer>();

        _tvMats[0] = _tvMat1;
        _tvMats[1] = _tvMat2;

        _tv.materials = _tvMats;
    }

    // Update is called once per frame
    void Update()
    {

  
    }
}
