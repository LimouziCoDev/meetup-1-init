using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {

    [SerializeField]
    private AudioClip[] attack;
    [SerializeField]
    private AudioClip[] hit;
    [SerializeField]
    private AudioClip[] dead;

    private new AudioSource audio;

    public void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void PlaySound(string type)
    {
        if(type == "attack")
        {
            PlayRandomSound(attack);
        }else if(type == "hit")
        {
            PlayRandomSound(hit);
        }else if(type == "dead")
        {
            PlayRandomSound(dead);
        }
    }

    private void PlayRandomSound(AudioClip[] list)
    {
        var clip = list[Random.Range(0, list.Length)];
        audio.PlayOneShot(clip);
    }
}
