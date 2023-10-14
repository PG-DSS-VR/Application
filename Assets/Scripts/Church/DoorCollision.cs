using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorCollision : MonoBehaviour
{
    public Animator doorAnim;
    bool doorOpen = false;

    public void buttonPressed() {
        if (doorAnim.GetBool("isOpen")) {
            doorAnim.SetBool("isOpen", false);
        } else {
            doorAnim.SetBool("isOpen", true);
        }
    }
}
