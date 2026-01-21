using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource permanentAmbientSource;
    [SerializeField] private AudioSource shortAmbientSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] forestAmbientSounds;
    [SerializeField] private AudioClip[] forestShortAmbientSounds;
    [SerializeField] private AudioClip[] forestMusicSounds;
    [SerializeField] private Vector2 timeBetweenShortAmbients;
    [SerializeField] private Vector2 timeBetweenMusic;

    private Location currentLocation;
    private Coroutine permanentAmbientC;

    public void setNewLocation(Location location)
    {
        currentLocation=location;
        StartCoroutine(StopCurrentAmbient());
    }


    void Start()
    {
        permanentAmbientC = StartCoroutine(permanentAmbient());
        StartCoroutine(shortAmbient());
        StartCoroutine(music());
    }



    private IEnumerator permanentAmbient()
    {
        while(true)
        {
            switch(currentLocation)
            {
                case Location.Forest:
                    permanentAmbientSource.Stop();
                    permanentAmbientSource.clip = forestAmbientSounds[Random.Range(0,forestAmbientSounds.Length)];
                    permanentAmbientSource.Play();
                    break;
                case Location.Desert:
                    break;
                case Location.Base:
                    break;
            }

            yield return new WaitForSeconds(permanentAmbientSource.clip.length);
        }
    }



    private IEnumerator StopCurrentAmbient()
    {
        while(true)
        {
            permanentAmbientSource.volume=permanentAmbientSource.volume-0.005f;
            yield return new WaitForSeconds(0.01f);

            if(permanentAmbientSource.volume<0.01f)
                break;
        }

        StopCoroutine(permanentAmbientC);
        permanentAmbientSource.Stop();
        permanentAmbientSource.volume=1f;
        permanentAmbientC = StartCoroutine(permanentAmbient());
    }



    private IEnumerator shortAmbient()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(timeBetweenShortAmbients.x,timeBetweenShortAmbients.y));

            switch(currentLocation)
            {
                case Location.Forest:
                    shortAmbientSource.Stop();
                    shortAmbientSource.clip = forestShortAmbientSounds[Random.Range(0,forestShortAmbientSounds.Length)];
                    shortAmbientSource.Play();
                    break;
                case Location.Desert:
                    break;
                case Location.Base:
                    break;
            }

            yield return new WaitForSeconds(shortAmbientSource.clip.length);
        }
    }



    private IEnumerator music()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(timeBetweenMusic.x,timeBetweenMusic.y));

            switch(currentLocation)
            {
                case Location.Forest:
                    musicSource.Stop();
                    musicSource.clip = forestMusicSounds[Random.Range(0,forestMusicSounds.Length)];
                    musicSource.Play();
                    break;
                case Location.Desert:
                    break;
                case Location.Base:
                    break;
            }

            yield return new WaitForSeconds(musicSource.clip.length);
        }
    }
}
