using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aani : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.K))
        {
             animator.SetBool("locon",true);
             
             //animator.Play("dodge_front");

        }
        else
        {
            animator.SetBool("locon",false);
            //animator.Play("New State");

        }



        if(Input.GetKey(KeyCode.J))
        {
             animator.SetBool("llocon",true);
             
             //animator.Play("dodge_front");

        }
        else
        {
            animator.SetBool("llocon",false);
            //animator.Play("New State");
        }
    }
}
