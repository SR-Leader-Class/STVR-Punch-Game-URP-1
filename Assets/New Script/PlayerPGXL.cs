using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPGXL : MonoBehaviour
{

    public GameObject leftHand;
    public GameObject rightHand;
    public Animator animator;
    public string preAnimation;

    public int healthValue;
    private Vector3 lastPositionR;
    public float rightHandSpeed, leftHandSpeed, ctrlPunchSpeed;
    public bool isPunch;

    public bool attackable = true;
    public int holdLimiteTime = 5, konckTime;
    public float takeDamageTime = 2f;
    
    void Start()
    {
        lastPositionR = rightHand.transform.position;
    }
    
    void Update()
    {

        Check_Hand_Speed();
    
        if(leftHandSpeed >= ctrlPunchSpeed && attackable == true)
            leftHand.name = "Player Left Hand";
        else
            leftHand.name = "Player Left Hand (Freeze)";

        if(rightHandSpeed >= ctrlPunchSpeed && attackable == true)
            rightHand.name = "Player Right Hand";
        else
            rightHand.name = "Player Right Hand (Freeze)";

        if(attackable == true)
            Animation("Idle");
        else
            Animation("Knockout");
    }

    void Check_Hand_Speed()
    {
        float distance = Vector3.Distance(rightHand.transform.position, lastPositionR);

        rightHandSpeed = distance / Time.deltaTime;
        lastPositionR = rightHand.transform.position;
    }

    void Animation(string animType)
    {

        if(animType == "Idle" && preAnimation != animType)
            animator.CrossFadeInFixedTime("idle_1", 0.1f);

        if(animType == "Knockout" && preAnimation != animType)
            animator.CrossFadeInFixedTime("knockout", 0.1f);

        preAnimation = animType;
    }

    public void Take_Damage()
    {
        if(attackable == true)
            StartCoroutine(take_damage());
    }

    IEnumerator take_damage()
    {
        GameControllerPGXL.Score --;
        Debug.Log("Player Get Hit");
        if(konckTime < holdLimiteTime)
            konckTime ++;
        else
        {
            attackable = false;
            konckTime = 0;
            yield return new WaitForSeconds(takeDamageTime);
            attackable = true;
        }
    }
}
