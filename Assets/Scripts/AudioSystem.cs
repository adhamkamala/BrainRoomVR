using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource audioSource;
    public void PlayPrimaryClickAudio()
    {
        audioSource.clip = GetAudioClipByName("click1");
        audioSource.Play();
    }
    public void PlaySecondaryClickAudio()
    {
        audioSource.clip = GetAudioClipByName("click2");
        audioSource.Play();
    }
    public void PlayPrimaryInfoAudio()
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
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
