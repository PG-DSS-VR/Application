using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuranExplanation : MonoBehaviour
{
    //public GameObject cube;
    public GameObject quran;
    public GameObject quranPOI;
    Animator quranAnimator;

    // Start is called before the first frame update
    void Start()
    {
        quranAnimator = quran.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitiateOpenQuran()
    {
        quran.SetActive(true);
        quranAnimator.SetBool("open", true);
    }

    public void InitiateCloseQuran()
    {
        StartCoroutine(CloseQuran());
    }

    public IEnumerator CloseQuran()
    {
        quranAnimator.SetBool("close", true);
        yield return new WaitForSeconds(5);
        quran.SetActive(false);
        quranPOI.SetActive(true);
        GlobalVarsIslam.interactiveElementActive = false;
    }
}
