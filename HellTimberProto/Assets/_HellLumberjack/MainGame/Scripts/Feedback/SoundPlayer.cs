using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public float volume = 0.7f;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    public bool destroySound = false;

    public void Play()
    {
        if (destroySound)
        {
            transform.SetParent(null);
            audioSource.enabled = true;
            Destroy(gameObject, 5);
        }

        audioSource.volume = volume;
        audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, Random.value);
        audioSource.PlayOneShot(audioClip);
    }
}
