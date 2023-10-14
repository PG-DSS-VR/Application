using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quran : MonoBehaviour
{
    public GameObject quran;
    public Animator quranAnimator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InitiateOpenQuran()
    {
        this.gameObject.SetActive(true);
        quranAnimator.SetBool("open", true);
    }

    public void InitiateCloseQuran()
    {
        StartCoroutine(CloseQuran());
    }

    public IEnumerator CloseQuran()
    {
        int currentState = Animator.StringToHash("Base Layer.Exit");
        quranAnimator.SetBool("close", true);
        while (!(quranAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash == currentState))
        {
            //do nothing
        }
        yield return new WaitForSeconds(1);
        quran.SetActive(false);
    }
}
