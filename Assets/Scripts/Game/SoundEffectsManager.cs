using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    // Audio clip
    public AudioClip chime;
    private AudioSource chimeSource;

    private void Awake()
    {
        if (chime != null)
        {
            chimeSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayChime()
    {
        if (chimeSource)
        {
            float volume = 0.4f;

            chimeSource.pitch = 0.9f;
            chimeSource.PlayOneShot(chime, volume);
        }
    }
}
