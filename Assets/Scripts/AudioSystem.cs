using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayNormalClickAudio()
    {
        audioSource.clip = GetAudioClipByName("click1");
        audioSource.Play();
    }
    public void PlaySecondaryClickAudio()
    {
        audioSource.clip = GetAudioClipByName("click2");
        audioSource.Play();
    }
    public void PlayNormalInfoAudio()
    {
        audioSource.clip = GetAudioClipByName("info1");
        audioSource.Play();
    }

    private AudioClip GetAudioClipByName(string clipName)
    {
        foreach (var clip in audioClips)
        {
            if (clip.name == clipName)
            {
                return clip;
            }
        }
        Debug.LogWarning($"Audio clip with name '{clipName}' not found.");
        return null;
    }


}
