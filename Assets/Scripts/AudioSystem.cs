using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public AudioClip[] soundClips;
    private AudioSource soundSource;
    public void PlayPrimaryClickSound()
    {
        soundSource.clip = GetSoundByTitel("click1");
        soundSource.Play();
    }
    public void PlaySecondaryClickSound()
    {
        soundSource.clip = GetSoundByTitel("click2");
        soundSource.Play();
    }
    public void PlayPrimaryInfoSound()
    {
        soundSource.clip = GetSoundByTitel("info1");
        soundSource.Play();
    }
    private AudioClip GetSoundByTitel(string clipTitel)
    {
        foreach (var sound in soundClips)
        {
            if (sound.name == clipTitel)
            {
                return sound;
            }
        }
        Debug.LogWarning($"Sound with name '{clipTitel}' not found.");
        return null;
    }
    private void Start()
    {
        soundSource = GetComponent<AudioSource>();
    }
}
