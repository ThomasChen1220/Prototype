using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    public AudioClip ringObtained1;
    public AudioClip ringObtained2;
    public AudioClip ringObtained3;
    public AudioClip explosion;

    [Header("Mixer Groups")]
    public AudioMixerGroup musicGroup;  //The music mixer group
    public AudioMixerGroup playerGroup; //The player mixer group
    public AudioMixerGroup explosionGroup;  //The voice mixer group
    
    AudioSource musicSource;            //Reference to the generated music Audio Source
    AudioSource playerSource;           //Reference to the generated player Audio Source
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

        playerSource.outputAudioMixerGroup = playerGroup;
        musicSource.outputAudioMixerGroup = musicGroup;
        explosionSource.outputAudioMixerGroup = explosionGroup;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void resetPickUp()
    {
        count = 0;
    }
    public void pickedUpSphereSound()
    {
        //audiosource.PlayOneShot(ringObtained1, .60f);

        if (count == 0)
        {
            playerSource.PlayOneShot(ringObtained1, .60f);
            count++;
        }
        else if (count == 1)
        {
            playerSource.PlayOneShot(ringObtained2, .60f);
            count++;
        }
        else if (count == 2)
        {
            playerSource.PlayOneShot(ringObtained3, .60f);
            count = 0;
        }

    }

    public void crashedSound()
    {
        explosionSource.PlayOneShot(explosion, .8f);
    }

}