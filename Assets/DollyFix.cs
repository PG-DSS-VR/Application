using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DollyFix : MonoBehaviour
{
    public CinemachineDollyCart parent;
    public GameObject go;
    private float startX;

    // Start is called before the first frame update
    void Start()
    {
        startX = go.transform.rotation.x;
    }

    // Update is called once per frame
    void Update()
    {
        go.transform.rotation = new Quaternion(startX, go.transform.rotation.y, go.transform.rotation.z, go.transform.rotation.w);
    }
}
