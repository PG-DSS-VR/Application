using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    public GameObject[] dynamicModels;
    public GameObject[] staticModels;
    GameObject[] models;

    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.transform.parent.name.Contains("Static"))
        {
            models = staticModels;
        }
        else
        {
            models = dynamicModels;
        }
        int index = Random.Range(0, models.Length);
        GameObject selectedBot = models[index];
        selectedBot.SetActive(true);
        Animator animator = selectedBot.GetComponent<Animator>();
        string boolName = "npc" + (index + 1);
        Debug.Log(boolName);
        animator.SetBool(boolName, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
