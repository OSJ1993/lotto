using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //������Ʈ�� Ȱ��ȭ �� �� ���� ����.
    private void OnEnable()
    {
        //������Ʈ�� Ȱ��ȭ �� �� �ڷ�ƾ ����.
        StartCoroutine("BallMove");
    }

    //���� ���� ���� Ƣ�� ������ �ڷ�ƾ
    IEnumerator BallMove()
    {
        while (true)
        {
            //ForceMode.Impulse �������� ��
            GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f))*30,ForceMode.Impulse);

            //Ƣ������� ������ 1��.
            yield return new WaitForSeconds(1);
        }
    }
}
