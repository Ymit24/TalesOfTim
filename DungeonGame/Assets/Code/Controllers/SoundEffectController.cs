using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    public static SoundEffectController Instance;
    private List<AudioSource> audioSources = new List<AudioSource>();
    public bool isEnabled;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioClip clip)
    {
        if (isEnabled == false) return;
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = false;
        source.Play();
        audioSources.Add(source);
    }

    private void Update()
    {
        // TODO: MAKE THIS BETTER
        List<AudioSource> finished = new List<AudioSource>();
        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying == false)
            {
                finished.Add(source);
            }
        }
        foreach(AudioSource source in finished)
        {
            audioSources.Remove(source);
        }
        finished.Clear();
    }
}
