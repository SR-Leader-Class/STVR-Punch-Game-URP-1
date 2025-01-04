using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAction : MonoBehaviour
{
    private Animator animator;
    private string[] actions = { "Action1", "Action2", "Action3", "Action4", "Action5", "Action6" };

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(PerformRandomActions());
    }

    IEnumerator PerformRandomActions()
    {
        for (int i = 0; i < 30; i++)  // 共進行30次動作
        {
            int actionIndex = Random.Range(0, actions.Length);  // 隨機選擇動作
            animator.SetTrigger(actions[actionIndex]);  // 觸發動作
            Debug.Log(actions[actionIndex]);
            yield return new WaitForSeconds(2);  // 每2秒進行一次
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
