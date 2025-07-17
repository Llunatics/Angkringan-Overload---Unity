using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public AudioClip bgmClip;
    public AudioClip masakClip;
    public AudioClip piringClip;
    public AudioClip cucipiringClip;
    public AudioClip stepsClip;
    public AudioClip tingClip;
    public AudioClip streetAmbienceClip;

    void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayMasak()
    {
        sfxSource.PlayOneShot(masakClip);
    }

    public void PlayPiring()
    {
        sfxSource.PlayOneShot(piringClip);
    }

    public void PlayCuciPiring()
    {
        sfxSource.PlayOneShot(cucipiringClip);
    }

    public void PlaySteps()
    {
        sfxSource.PlayOneShot(stepsClip);
    }

    public void PlayTing()
    {
        sfxSource.PlayOneShot(tingClip);
    }

    public void PlayStreetAmbience()
    {
        sfxSource.PlayOneShot(streetAmbienceClip);
    }
}
