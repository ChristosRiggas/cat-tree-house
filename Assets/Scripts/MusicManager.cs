using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip jumpRopeMusic;
    public AudioClip butterflyMusic;
    public AudioClip fishCollectMusic;

    [Header("SFX Clips")]
    public AudioClip failSFX;
    public AudioClip winSFX;
    public AudioClip coinSFX;
    public AudioClip eatSFX;
    public AudioClip jumpSFX;


    private Coroutine fadeRoutine;


    private bool isInLobby = true;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (Instance != null)
            PlayMainMenu();
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        AudioClip nextClip = GetClipForScene(newScene.name);
        musicSource.loop = true;
        if (nextClip == mainMenuMusic)
        {
            isInLobby = true;
            PlayMainMenu();
            return;
        }
        else 
        {
            isInLobby = false;
        }
        if (nextClip != musicSource.clip)
        {
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);

            fadeRoutine = StartCoroutine(FadeAndSwitch(nextClip));
        }
    }

    private AudioClip GetClipForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Lobby":
                return mainMenuMusic;

            case "MiniGame1Scene":
                return jumpRopeMusic;

            case "PlatformGame":
                return butterflyMusic;

            case "MiniGameCatchFish":
                return fishCollectMusic;
            default:
                return mainMenuMusic;
        }
    }

    public void PlaySFX()
    {
        AudioClip clip;
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "MiniGame1Scene":
                clip = jumpSFX;
                break;
            case "PlatformGame":
                clip = coinSFX;
                break;
            case "MiniGameCatchFish":
                clip = eatSFX;
                break;
            default:
                clip = coinSFX;
                break;
        }
        sfxSource.PlayOneShot(clip);
    }


    public void PlayEndSFX(bool hasWon)
    {

        musicSource.loop = false;
        AudioClip nextClip = failSFX;
        if (hasWon && winSFX != null)
        {
           nextClip = winSFX;
        }
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeAndSwitch(nextClip, 0.5f));
    }

    public void PlayMainMenu()
    {
        PlayTrack(mainMenuMusic);
    }

    private void PlayTrack(AudioClip clip)
    {
        if (clip == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    private IEnumerator FadeAndSwitch(AudioClip newClip, float fadeTime = 1f)
    {
        float startVolume = musicSource.volume;

        // Fade out
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newClip;

        if (newClip != null)
        {
            musicSource.Play();

            // Fade in
            t = 0f;
            while (t < fadeTime)
            {
                t += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(0f, startVolume, t / fadeTime);
                yield return null;
            }
        }

        musicSource.volume = startVolume;
    }
}