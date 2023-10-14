using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talking : MonoBehaviour
{
    public AudioClip firstClip;
    public AudioClip second;
    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        StartCoroutine(playFirst());
    }

    // Update is called once per frame
    IEnumerator playFirst() {
        audio.PlayOneShot(firstClip);
        DateTime now = DateTime.Now;
        yield return new WaitForSeconds(30);

        playSecond();
    }

    void playSecond() {
        audio.PlayOneShot(second);
    }
}
