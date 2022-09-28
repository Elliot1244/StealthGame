using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundScript : MonoBehaviour
{
    [SerializeField] AudioMixer _mixer;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerMouvement>() != null)
        {
            _mixer.SetFloat("StrangeManVoice", 40f);

            //_mixer.SetFloat("RainSoundScape", 40f);
            _mixer.SetFloat("RainVolume", -5f);
        }
    }

}
