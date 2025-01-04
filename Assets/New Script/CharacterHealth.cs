using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{

    public Mode ModeSet = Mode.Player;
    public enum Mode
    {
        Player, Enemy
    };
    
    public PlayerPGXL player;
    public EnemyMovement enemyMovement;

    public Collider[] hits;
    public float checkDistance;

    bool damageLock = true;
    
    void Start()
    {
        
    }
    
    void Update()
    {

        if(ModeSet == Mode.Player)
        {
            hits = Physics.OverlapSphere(this.transform.position, checkDistance);

            foreach(Collider hit in hits)
            {
                if((hit.name == "Bip001 L Hand001" || hit.name == "Bip001 R Hand001") && damageLock == true)
                {
                    damageLock = false;
                    StartCoroutine(delay());
                    player.Take_Damage();
                }
            }
        }

        IEnumerator delay()
        {
            yield return new WaitForSeconds(1f);
            damageLock = true;
        }

        //rb.velocity.magnitude
        if(ModeSet == Mode.Enemy)
        {
            hits = Physics.OverlapSphere(this.transform.position, checkDistance);

            foreach(Collider hit in hits)
            {
                if((hit.name == "Player Left Hand" || hit.name == "Player Right Hand") && damageLock == true)
                {
                    damageLock = false;
                    enemyMovement.Take_Damage();
                }
            }
        }

    }

    public void Damage()
    {
        //if(isPlayer == true)
        //    player.Take_Damage();
        //playerMovement.Take_Damage();
    }
}
