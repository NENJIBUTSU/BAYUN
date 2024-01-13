using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioSource musicAudioSource;
    [SerializeField] public AudioSource audioSource;

    public static AudioManager Instance { get; private set; }

    private void Awake() {

        if (Instance != null && Instance != this) {
            Destroy(this);
            Instance = null;
        }
        else { Instance = this; DontDestroyOnLoad(gameObject); }
    }

    public void PlaySound(AudioContainer audio) {
        if(audio.type == AudioType.Music) {
            Debug.Log("Attempted to play music in normal audio clip!");
            return;
        }

        audioSource.PlayOneShot(audio.clip);
    }
    public void PlaySound(AudioContainer audio, float volume) {
        if (audio.type == AudioType.Music) {
            Debug.Log("Attempted to play music in normal audio clip!");
            return;
        }

        audioSource.PlayOneShot(audio.clip, volume);
    }

    public void PlayMusic(AudioContainer audio) {
        musicAudioSource.Play();
    }

    public void DestroySelf() {
        Destroy(this.gameObject);
        Instance = null;
    }
}

public struct AudioContainer {
    public AudioType type;
    public AudioClip clip;

}

public enum AudioType {
    Music,
    SFX
}