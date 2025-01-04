using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jawPos : MonoBehaviour
{
    public Vector3 jawposition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        jawposition = this.transform.position;
    }
}
