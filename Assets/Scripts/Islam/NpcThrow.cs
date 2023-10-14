using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcThrow : MonoBehaviour
{

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(ThrowRandomly());
    }

    private IEnumerator ThrowRandomly()
    {
        int randomWait = Random.Range(9, 20);
        yield return new WaitForSeconds(randomWait);
        animator.SetBool("throw", true);
        yield return new WaitForSeconds(2);
        animator.SetBool("throw", false);
    }
}
