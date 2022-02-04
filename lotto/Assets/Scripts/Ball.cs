using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //프로젝트가 활성화 될 때 마다 실행.
    private void OnEnable()
    {
        //오브젝트가 활성화 될 때 코루틴 실행.
        StartCoroutine("BallMove");
    }

    //공이 위로 통통 튀어 오르는 코루틴
    IEnumerator BallMove()
    {
        while (true)
        {
            //ForceMode.Impulse 순간적인 힘
            GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f))*30,ForceMode.Impulse);

            //튀어오르는 간격은 1초.
            yield return new WaitForSeconds(1);
        }
    }
}
