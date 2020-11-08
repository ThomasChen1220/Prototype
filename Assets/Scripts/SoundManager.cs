using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    public AudioClip[] ringObtained;
    public AudioClip trapSound;
    public AudioClip missSound;
    public AudioClip explosion;
    public AudioClip mouseClick;
    public AudioClip mouseHover;
    public AudioClip menuBGM;
    public AudioClip gameBGM;

    [Header("Mixer Groups")]
    public AudioMixerGroup musicGroup;  //The music mixer group
    public AudioMixerGroup playerGroup; //The player mixer group
    public AudioMixerGroup explosionGroup;  //The voice mixer group
    public AudioMixerGroup UIGroup;
    
    AudioSource musicSource;            //Reference to the generated music Audio Source
    AudioSource playerSource;           //Reference to the generated player Audio Source
    AudioSource UISource;           //Reference to the generated player Audio Source
    AudioSource explosionSource;            //Reference to the generated voice Audio Source

    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);

        playerSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        musicSource = gameObject.GetComponent<AudioSource>();
        explosionSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        UISource = gameObject.AddComponent<AudioSource>() as AudioSource;

        playerSource.outputAudioMixerGroup = playerGroup;
        musicSource.outputAudioMixerGroup = musicGroup;
        explosionSource.outputAudioMixerGroup = explosionGroup;
        UISource.outputAudioMixerGroup = UIGroup;
    }
    public void PlayMenuBGM() {
        if (musicSource.clip != menuBGM)
        {
            musicSource.Stop();
            musicSource.clip = menuBGM;
            musicSource.Play();
        }
        
    }
    public void PlayGameBGM()
    {
        if (musicSource.clip != gameBGM)
        {
            musicSource.Stop();
            musicSource.clip = gameBGM;
            musicSource.Play();
        }
    }
    public void PlayMouseHoverSound()
    {
        UISource.PlayOneShot(mouseHover);
    }
    public void PlayMouseClickSound()
    {
        UISource.PlayOneShot(mouseClick);
    }
    public void PlayMissedSound()
    {
        playerSource.PlayOneShot(missSound);
    }

    public void PlayTrapSound()
    {
        //playerSource.PlayOneShot(trapSound);
    }
    public bool CheckMaxed()
    {
        return count >= (ringObtained.Length);
    }
    public void resetPickUp()
    {
        count = 0;
        GameManager.instance.currPlayer.GetComponent<PlaceTraps>().TurnOffTrail();
    }
    public void PlayPickedUpSphereSound()
    {

        count = Mathf.Min(count, ringObtained.Length-1);
        //audiosource.PlayOneShot(ringObtained1, .60f);
        playerSource.PlayOneShot(ringObtained[count], .60f);
        count++;
    }

    public void PlayCrashedSound()
    {
        explosionSource.PlayOneShot(explosion, .8f);
    }

}