using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfessionalOpen : MonoBehaviour
{
    public Animator curtainAnim;
    bool initialOpen = true;

    public void open() {
        if (initialOpen) {
            initialOpen = false;
            return;
        }
        curtainAnim.SetBool("left_curtain_open", true);
    }

    public void openAll() {
        curtainAnim.SetBool("left_curtain_open", true);
        curtainAnim.SetBool("front_door_open", true);
        curtainAnim.SetBool("front_curtain_open", true);
    }

    public void closeAll() {
        curtainAnim.SetBool("left_curtain_open", false);
        curtainAnim.SetBool("front_door_open", false);
        curtainAnim.SetBool("front_curtain_open", false);
    }

    public void close() {
        curtainAnim.SetBool("left_curtain_open", false);
    }
}
