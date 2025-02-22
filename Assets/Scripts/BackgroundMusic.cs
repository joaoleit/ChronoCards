using System.Collections;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public float fadeDuration = 2f;
    public float switchMusicFadeDuration = 2f;
    public AudioClip backgroundMusic; // Música de fundo (ambiente)
    public AudioClip battleMusic; // Música de batalha

    private AudioSource audioSource;
    private float targetVolume;

    public static BackgroundMusic Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        targetVolume = audioSource.volume;

        PlayMusic(backgroundMusic);
    }

    void Update()
    {
        // if (GameManager.Instance != null)
        // {
        //     if (GameManager.Instance.isWorldActive && audioSource.clip != backgroundMusic)
        //     {
        //         StartCoroutine(SwitchMusic(backgroundMusic));
        //     }
        //     else if (!GameManager.Instance.isWorldActive && audioSource.clip != battleMusic)
        //     {
        //         StartCoroutine(SwitchMusic(battleMusic));
        //     }
        // }
    }

    private void PlayMusic(AudioClip music)
    {
        audioSource.clip = music;
        audioSource.volume = 0f;
        audioSource.Play();
        StartCoroutine(FadeIn(fadeDuration));
    }

    public void SwitchToBattleMusic()
    {
        StartCoroutine(SwitchMusic(battleMusic));
    }

    public void SwitchToWorldMusic()
    {
        StartCoroutine(SwitchMusic(backgroundMusic));
    }

    private IEnumerator SwitchMusic(AudioClip newMusic)
    {
        // yield return StartCoroutine(FadeOut(2));

        // yield return new WaitForSeconds(2f);
        audioSource.clip = newMusic;
        audioSource.volume = 0;
        audioSource.Play();

        yield return StartCoroutine(FadeIn(10));
    }

    private IEnumerator FadeIn(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            audioSource.volume = Mathf.Lerp(0f, targetVolume, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    private IEnumerator FadeOut(float duration)
    {
        float elapsedTime = 0f;
        float startVolume = audioSource.volume;

        while (elapsedTime < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0f;
    }
}