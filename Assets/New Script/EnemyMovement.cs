using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class EnemyMovement : MonoBehaviour
{

    public GameObject leftHand;
    public GameObject rightHand;

    private Animator animator;
    public string preAnimation;

    private NavMeshAgent agent;
    public GameObject targetPoint;
    public Collider[] hits;
    public GameObject shooterPoint;
    public int enemyNearNum;
    public float speed, checkDistance, range;
    public bool isBagArrive, moveable = true;

    public float disAttack, ctrlAttackDis, ctrlEnemyNearPlayerDis, disEnemyNearPlayer;

    public float attackLoopTime = 2, attackIdleLoopTime = 0.5f;
    private bool attackLock = true, isDead;


    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponentInChildren<NavMeshAgent>();
        leftHand.name = "Bip001 L Hand001 (Freeze)";
        rightHand.name = "Bip001 R Hand001 (Freeze)";
    }


    void Update()
    {

        Check_Enemy_Density();

        agent.speed = speed;
        disAttack = Vector3.Distance(targetPoint.transform.position, this.transform.position);

        if(moveable == true)
        {
            if(isDead == true)
            {
                agent.SetDestination(this.transform.position);
                Animation("Knockout");
            }
            else if(disAttack <= ctrlAttackDis)
            {
                agent.SetDestination(this.transform.position);
                Look_Direction();
                if(attackLock == true)
                    StartCoroutine(attack());
            }
            else if(enemyNearNum >= 4 && disEnemyNearPlayer >= ctrlEnemyNearPlayerDis)
            {
                agent.SetDestination(this.transform.position);
                Animation("Idle");
            }
            else
            {
                agent.SetDestination(targetPoint.transform.position);
                Animation("Walk");
            }
        }
        
        //else
        //{
        //    agent.SetDestination(this.transform.position);
        //    Animation("Idle");
        //}
    }

    void Check_Enemy_Density()
    {

        disEnemyNearPlayer = Vector3.Distance(targetPoint.transform.position, this.transform.position);

        hits = Physics.OverlapSphere(this.transform.position, checkDistance);

        enemyNearNum = hits.Count(hit => hit.gameObject.CompareTag("Enemy"));
    }

    IEnumerator attack()
    {
        attackLock = false;
        //Shoot();
        leftHand.name = "Bip001 L Hand001 (Freeze)";
        rightHand.name = "Bip001 R Hand001 (Freeze)";

        int randomNum = Random.Range(0, 2 + 1);
        if(randomNum == 0)
            Animation("Jab Left");
        if(randomNum == 1)
            Animation("Jab Right");

        yield return new WaitForSeconds(0.05f);
        leftHand.name = "Bip001 L Hand001";
        rightHand.name = "Bip001 R Hand001";

        yield return new WaitForSeconds(attackLoopTime);
        Animation("Idle");
        yield return new WaitForSeconds(attackIdleLoopTime);

        leftHand.name = "Bip001 L Hand001 (Freeze)";
        rightHand.name = "Bip001 R Hand001 (Freeze)";

        attackLock = true;
    }

    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(shooterPoint.transform.position, shooterPoint.transform.forward, out hit, range))
        {          
            CharacterHealth characterHealth = hit.transform.GetComponent<CharacterHealth>();
            if(characterHealth != null)
            {
                characterHealth.Damage();
            }
        }
    }

    void Animation(string animType)
    {

        if(animType == "Idle" && preAnimation != animType)
            animator.CrossFadeInFixedTime("idle_1", 0.1f);

        if(animType == "Walk" && preAnimation != animType)
            animator.CrossFadeInFixedTime("walk_forward", 0.1f);

        if(animType == "Jab Left")
            animator.CrossFadeInFixedTime("jab_left", 0.1f);

        if(animType == "Jab Right")
            animator.CrossFadeInFixedTime("jab_right", 0.1f);

        if(animType == "Knockout" && preAnimation != animType)
            animator.CrossFadeInFixedTime("knockout", 0.1f);

        if(animType == "Dodge Front")
            animator.CrossFadeInFixedTime("dodge_front", 0.1f);

        preAnimation = animType;
    }

    public void Take_Damage()
    {
        GameControllerPGXL.Score ++;
        if(isBagArrive == true)
            Dead();
        else
            StartCoroutine(dodge());
    }

    IEnumerator dodge()
    {
        moveable = false;
        Animation("Dodge Front");
        yield return new WaitForSeconds(0.33f);
        moveable = true;
    }

    void Dead()
    {
        isDead = true;
        StartCoroutine(disappear());
    }

    IEnumerator disappear()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }

    void Look_Direction()
    {
        Vector3 direction = targetPoint.transform.position - this.transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        this.transform.rotation = Quaternion.LookRotation(direction);
    }
}
