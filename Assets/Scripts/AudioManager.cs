using UnityEngine;

public class AudioManager : MonoBehaviour {
    public Sound[] sounds;
    public static AudioManager Instance;

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
        }
    }

    public void Play(string name) {
        Sound sound = System.Array.Find(sounds, s => s.name == name);
        if (sound == null) {
            return;
        }
        sound.source.Play();
    }
}