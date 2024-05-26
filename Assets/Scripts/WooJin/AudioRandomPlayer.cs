using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioRandomPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> audioClips = new List<AudioClip>();

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandom() {
        int i = Random.Range(0, audioClips.Count);
        audioSource.Stop();
        audioSource.clip = audioClips[i];
        audioSource.Play();
    }

}