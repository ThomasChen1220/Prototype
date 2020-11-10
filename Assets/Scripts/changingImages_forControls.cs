using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changingImages_forControls : MonoBehaviour
{
    private float time = 2f;

    public GameObject WASDimage;
    public GameObject Arrowsimage;

    void Start()
    {
        WASDimage.SetActive(true);
        Arrowsimage.SetActive(false);

        StartCoroutine(Call());
    }

    IEnumerator Call()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(time);
            WASDimage.SetActive(false);
            Arrowsimage.SetActive(true);
            yield return new WaitForSeconds(time);
            WASDimage.SetActive(true);
            Arrowsimage.SetActive(false);
        }
    }
}
