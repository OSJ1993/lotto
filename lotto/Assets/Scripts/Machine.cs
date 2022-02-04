using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Machine : MonoBehaviour
{
    //�ζ� ������ �ڸ� �����.
    public GameObject[] ball;

    //�����Ǵ� �ζ� ���� ����
    int ballNum;

    //���� �� �����Ǹ� ������ �۾� �� �ٲٱ�.
    public TextMeshPro billBoard;

    //�غ���� Ȯ��.
    bool ready;

    //�ⱸ �ڸ�.
    public Transform exit;

    //��÷��ȣ List
    List<int> winningNum = new List<int>();

    //��÷��ȣ �������ؽ�Ʈ �ڸ�.
    public TextMeshPro[] num;

    //���� �� �����Ǹ� SetBall���� ����.
    public GameObject sound_SetBall;

    //Ready ����.
    public GameObject sound_Ready;

    //���� ������ ���� ����.
    public AudioSource sound_GetBall;

    //��ư ����.
    public AudioSource sound_Btn;

    private void Start()
    {
        StartCoroutine("StartMachine");
    }

    void SetBall()
    {
        // i�� 0���� �� ���� ��ŭ �ݺ� ����.
        for (int i = 0; i < ball.Length; i++)
        {
            //i��° ���� Ȱ��ȭ �Ǿ� ���� �ʴٸ� i���� ���� Ȱ��ȭ.
            if (!ball[i].activeSelf)
            {
                ball[i].SetActive(true);
                return;

            }
        }
    }

    //���ʴ�� ���� Ȱ��ȭ ������ �ڷ�ƾ
    IEnumerator StartMachine()
    {
        while (true)
        {
            //���� �����Ǵ� �������� SetBall ����. SetBall�� ����� �� ���� ballNum 1�� �����ֱ�.
            yield return new WaitForSeconds(0.1f);
            SetBall();
            ballNum += 1;

            //ballNum�� 45�� �Ǹ� �ڷ�ƾ ����.
            if (ballNum >= 45)
            {
                StopCoroutine("StartMachine");

                //�� 45���� ��� ������ �Ǹ� �����忡 �ؽ�Ʈ�� Ready�� �ٲٱ�.
                billBoard.text = "Ready";
                ready = true;

                //���� 45�� �� �����Ǹ� sound_SetBall ����.
                sound_SetBall.SetActive(false);

                //�� 45���� �� �����Ǹ� sound_Ready ����.
                sound_Ready.SetActive(true);
            }
        }
    }

    //��ư�� ������ ��÷�ǵ��� �ϴ� ��ư ���.
    public void PressGetBtn()
    {
        //��ư�� ���� �� ���� ���.
        sound_Btn.Play();

        //ready�� true�� ���� ��� ������ �Ķ�� ��÷ �ڷ�ƾ ����.
        if (ready)
        {
            //Get ��ư�� ������ ������ �ѹ��� ����.
            ready = false;

            StartCoroutine("Drow");

            //Get ��ư�� ������ �ϴ� billBoard �ؽ�Ʈ ��Ȱ��ȭ.
            billBoard.gameObject.SetActive(false);

        }
    }

    //��õ 6�� ���� �ڷ�ƾ.
    IEnumerator Drow()
    {
        //��÷ List�� �ִ� ���ڰ� 6���̸��� ���ȸ� ����.
        while (winningNum.Count < 6)
        {
            //r�� �ִ� ��õ ����� ����.
            GetBall();
            yield return new WaitForSeconds(2);
        }
    }


    //��÷ ���.
    void GetBall()
    {
        //0���� ball(44)�߿� ����.
        int r = Random.Range(0, ball.Length);

        //r+1 ���ڰ� �̹� ��÷ List�� �ִٸ� ��÷ ����� �ٽ� ����.
        if (winningNum.Contains(r + 1))
        {
            GetBall();
            return;
        }

        //�׷��� ���� r�� 1�� ���� ���ڰ� ��÷ List�ȿ� ���ٸ� ��÷ List�� r�� ���ϱ� 1�� �� ���ڸ� �ֽ��ϴ�.
        if (!winningNum.Contains(r + 1))
        {
            winningNum.Add(r + 1);

            //�׸��� r��°�� ���� ����ũ��Ʈ�� ������ �ͼ� �� �̻� ���� Ƣ�� ���ϰ� ��� �ڷ�ƾ ����.
            ball[r].GetComponent<Ball>().StopAllCoroutines();

            //r��° Rigidbody�� �����ͼ� �� �̻� �������� �ʰ� vilocity���� ���η� ����.
            ball[r].GetComponent<Rigidbody>().velocity = Vector3.zero;

            //���� ��Ų ���� �ⱸ�� �ű��.
            //r��° ���� �������� �ⱸ�� ���������� ������.
            ball[r].transform.position = exit.position;

            //���� ��÷�Ǽ� ���� ��.
            sound_GetBall.Play();

            //������ ��÷ ���� ��ŭ �ݺ� ����.
            for (int i = 0; i < num.Length; i++)
            {
                //������Ʈ�� Ȱ��ȭ �Ǿ� ���� �ʴ� ��쿡��.
                if (!num[i].gameObject.activeSelf)
                {

                    //i���� ������ ������ �ؽ�Ʈ�� r+1�� ����
                    num[i].text = string.Format("{0}", r + 1);

                    //i��° ������ ���� Ȱ��ȭ.
                    num[i].gameObject.SetActive(true);
                    return;

                }

            }

        }
    }

    //reset ��ư.
    public void PressResetBtn()
    {
        //��ư�� ���� �� ���� ���.
        sound_Btn.Play();

        //��ư�� ������ �� Invoke�� ���� ����.
        Invoke("ReStartBtn", 0.5f);

    }

    //��� �� ����� ���.
    void ReStartBtn()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
