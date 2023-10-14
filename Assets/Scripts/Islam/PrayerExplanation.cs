using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrayerExplanation : MonoBehaviour
{
    public GameObject prayerBot;
    public Animator botAnimator;
    public AudioSource audioPlayer;
    public GameObject poiPrayer;

    public AudioClip[] clips;

    bool canceled = false;

    // Start is called before the first frame update
    void Start()
    {
        canceled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitiatePrayer()
    {
        StartCoroutine(ExplainPrayer());
    }

    public IEnumerator ExplainPrayer()
    {
        this.gameObject.GetComponent<followPlayerIslam>().SetPreviousClipToPrayer();
        prayerBot.SetActive(true);

        int waitTime;
        string step;

        for(int i=0; i<17; i++)
        {

            step = "step" + (i);
            waitTime = (Mathf.FloorToInt(clips[i].length)) + 1;

            if(i > 0)
            {
                botAnimator.SetBool(step, true);
            }
            audioPlayer.clip = clips[i];
            audioPlayer.Play();
            yield return new WaitForSeconds(waitTime);

            if(canceled)
            {
                i = 17;
            }
        }

        Debug.Log("PRAYER BOT BEFORE:" + prayerBot);
        prayerBot.SetActive(false);
        Debug.Log("POI PRAYER BEFORE:" + poiPrayer);
        poiPrayer.SetActive(true);
        Debug.Log("POI PRAYER AFTER:" + poiPrayer);
        canceled = false;
        GlobalVarsIslam.interactiveElementActive = false;
        Debug.Log("after globalvars");
    }

    public void CancelExplanation()
    {
        canceled = true;
        StopAllCoroutines();
        prayerBot.SetActive(false);
        GlobalVarsIslam.interactiveElementActive = false;
        canceled = false;
    }
}
