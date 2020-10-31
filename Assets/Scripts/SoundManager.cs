using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource audiosource;
    public AudioClip ringObtained1;
    public AudioClip ringObtained2;
    public AudioClip ringObtained3;
    public AudioClip explosion;

    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();

        if (instance != null)
            Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);
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
            audiosource.PlayOneShot(ringObtained1, .60f);
            count++;
        }
        else if (count == 1)
        {
            audiosource.PlayOneShot(ringObtained2, .60f);
            count++;
        }
        else if (count == 2)
        {
            audiosource.PlayOneShot(ringObtained3, .60f);
            count = 0;
        }

    }

    public void crashedSound()
    {
        audiosource.PlayOneShot(explosion, .8f);
    }

}