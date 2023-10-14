using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class kaabaBots : MonoBehaviour {
    public CinemachineDollyCart bot1;
    public CinemachineDollyCart bot2;
    public CinemachineDollyCart bot3;
    public CinemachineDollyCart bot4;
    public CinemachineDollyCart bot5;
    public CinemachineDollyCart bot6;
    public Animator bot1_anim;
    public Animator bot2_anim;
    public Animator bot3_anim;
    public Animator bot4_anim;
    public Animator bot5_anim;
    public Animator bot6_anim;

    public void setSpeed(int i) {
        bot1.m_Speed = i;
        bot2.m_Speed = i;
        bot3.m_Speed = i;
        bot4.m_Speed = i;
        bot5.m_Speed = i;
        bot6.m_Speed = i;
        bot1_anim.SetBool("walk", true);
        bot2_anim.SetBool("walk", true);
        bot3_anim.SetBool("walk", true);
        bot4_anim.SetBool("walk", true);
        bot5_anim.SetBool("walk", true);
        bot6_anim.SetBool("walk", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
