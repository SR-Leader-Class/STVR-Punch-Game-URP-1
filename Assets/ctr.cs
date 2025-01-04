using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ctr : MonoBehaviour
{
    public GameObject dd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position=new Vector3(dd.transform.position.x,dd.transform.position.y,dd.transform.position.z);
        transform.rotation=dd.transform.rotation;
    }
}
