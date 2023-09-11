using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private List<AudioClip> backgroundList;

    private AudioSource background;
    private AudioSource sfx;

    private void Start()
    {
        background = transform.GetChild(0).GetComponent<AudioSource>();
        sfx = transform.GetChild(1).GetComponent<AudioSource>();

        Play(0);
    }

    public void Play(int _track)
    {
        background.clip = backgroundList[_track];
        background.Play();
    }
    public void PlayEffect(AudioClip _clip)
    {
        sfx.PlayOneShot(_clip);
    }
}
