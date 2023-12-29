using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Animator animator;
    // Update is called once per frame
    void Update()
    {
        bool mouseClick = Input.GetButtonDown("Fire1");

        if(mouseClick) {
            animator.SetBool("Attack", true);
        }
        else {
            animator.SetBool("Attack", false);
        }
    }
}
