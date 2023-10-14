using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossCovering : MonoBehaviour
{

    public Animator cloth;

    public void fallDown() {
        cloth.SetBool("isDown", true);
    }

    public void resetCloth() {
        cloth.SetBool("isDown", false);
    }
}
