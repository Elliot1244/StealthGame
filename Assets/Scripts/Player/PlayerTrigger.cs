using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] Material[] _tvMats;
    [SerializeField] Material _tvMat1;
    [SerializeField] Material _tvMat2;
    [SerializeField] Material _tvMat3;  
    [SerializeField] GameObject _tv1;
    [SerializeField] AudioSource _tvNoise;
    [SerializeField] GameObject _screamerTrigger;
    [SerializeField] GameObject _destroyTvTrigger;

    // Start is called before the first frame update
    void Start()
    {
        _screamerTrigger.SetActive(false);
    }



    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerMouvement>() != null)
        {
            Renderer _tv = _tv1.GetComponent<Renderer>();

            _tvMats[0] = _tvMat1;
            _tvMats[1] = _tvMat2;
            _tv.sharedMaterials = _tvMats;

            _tvNoise.Play();

            
            StartCoroutine("ShutDownTV");
            StartCoroutine("DestroyCollider");



        }
    }

    IEnumerator ShutDownTV()
    {
        yield return new WaitForSeconds(3);
        Renderer _tv = _tv1.GetComponent<Renderer>();
        _screamerTrigger.SetActive(true);
        _tvNoise.Stop();
        _tvMats[1] = _tvMat3;
        _tv.sharedMaterials = _tvMats;
        yield break;
    }

    IEnumerator DestroyCollider()
    {
        yield return new WaitForSeconds(3.1f);
        _destroyTvTrigger.SetActive(false);
    }
}
