using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ddmove : MonoBehaviour
{
    public GameObject dd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position=new Vector3(dd.transform.position.x,0f,dd.transform.position.z);
        transform.rotation=dd.transform.rotation;
    //     Vector3 eulerAngles = transform.eulerAngles; // 獲取物件a的歐拉角
    // eulerAngles.y = dd.transform.eulerAngles.z; // 將Y軸設置為物件b的Y軸旋轉值
    // transform.eulerAngles = eulerAngles; // 更新物件a的旋轉
    }
}
