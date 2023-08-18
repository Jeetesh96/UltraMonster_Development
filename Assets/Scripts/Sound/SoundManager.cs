using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] music;
    private AudioSource audiosource;
    

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }
    public void PlayClick()
    {
        audiosource.PlayOneShot(music[0]);
    }
    public void OnDrop()
    {
        audiosource.PlayOneShot(music[1]);
    }
    public void Bombblast()
    {
        audiosource.PlayOneShot(music[2]);
    }
    public void RoundBell()
    {
        audiosource.PlayOneShot(music[3]);
    }
    public void Applause()
    {
        audiosource.PlayOneShot(music[4]);
    }
    public void DiceRoll()
    {
        audiosource.PlayOneShot(music[5]);
    }
}
