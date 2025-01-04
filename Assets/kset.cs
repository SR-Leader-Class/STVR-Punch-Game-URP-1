using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kset : MonoBehaviour
{
    public GameObject Player;
    public GameObject bat;
    public bool soak;
    public Vector3 distance;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(soak){
            distance=bat.transform.position-Player.transform.position;
            distance=bat.transform.position+distance;
            soak=false;
        }
    }
}
