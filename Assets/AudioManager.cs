using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("=========== audio source ===========")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXsource;

    [Header("=========== audio clip =============")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip finish;
    public AudioClip jump;
    public AudioClip walk;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, bool loop = false)
    {
        SFXsource.clip = clip;
        SFXsource.loop = loop;  // Allow looping for walk sound
        SFXsource.Play();
    }
    public void StopSFX()
    {
        SFXsource.Stop();
    }

}
