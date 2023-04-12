using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceManager : MonoBehaviour
{
    public static AmbianceManager Instance;
    [SerializeField] private AudioSource music;

    private float maxVolume;
    private bool isPlayingMusic;

    Coroutine c_StopMusic;

    private void Awake()
    {
        Instance = this;
        maxVolume = music.volume;
    }

    private void Start()
    {
        isPlayingMusic = true;
    }

    void Update()
    {
        if (isPlayingMusic)
            music.volume = Mathf.Lerp(music.volume, maxVolume, 0.07f);
        else
            music.volume = Mathf.Lerp(music.volume, 0.001f, 1f);
    }

    public void StopMusic(float duration)
    {
        isPlayingMusic = false;

        if (c_StopMusic != null)
            StopCoroutine(c_StopMusic);

        c_StopMusic = StartCoroutine(C_StopMusic(duration));
    }

    IEnumerator C_StopMusic(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        isPlayingMusic = true;
    }
}
