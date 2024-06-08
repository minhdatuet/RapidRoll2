using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    // Start is called before the first frame update
    public AudioClip deathSound;
    public AudioClip expSound;
    public AudioClip getSound;
    public AudioClip gunSound;
    public AudioClip overSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }

    public void PlayExpSound()
    {
        if (expSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(expSound);
        }
    }

    public void PlayGetSound()
    {
        if (getSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(getSound);
        }
    }

    public void PlayGunSound()
    {
        if (gunSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(gunSound);
        }
    }

    public void PlayOverSound()
    {
        if (overSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(overSound);
        }
    }
}
