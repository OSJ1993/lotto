using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Machine : MonoBehaviour
{
    //로또 볼들의 자리 만들기.
    public GameObject[] ball;

    //생성되는 로또 볼의 갯수
    int ballNum;

    //공이 다 생성되면 전광판 글씨 색 바꾸기.
    public TextMeshPro billBoard;

    //준비상태 확인.
    bool ready;

    //출구 자리.
    public Transform exit;

    //당첨번호 List
    List<int> winningNum = new List<int>();

    //당첨번호 전광판텍스트 자리.
    public TextMeshPro[] num;

    //공이 다 생성되면 SetBall사운드 종료.
    public GameObject sound_SetBall;

    //Ready 사운드.
    public GameObject sound_Ready;

    //공이 나오는 동안 사운드.
    public AudioSource sound_GetBall;

    //버튼 사운드.
    public AudioSource sound_Btn;

    private void Start()
    {
        StartCoroutine("StartMachine");
    }

    void SetBall()
    {
        // i는 0부터 볼 갯수 만큼 반복 실행.
        for (int i = 0; i < ball.Length; i++)
        {
            //i번째 공이 활성화 되어 있지 않다면 i번쨰 공을 활성화.
            if (!ball[i].activeSelf)
            {
                ball[i].SetActive(true);
                return;

            }
        }
    }

    //차례대로 공을 활성화 시켜줄 코루틴
    IEnumerator StartMachine()
    {
        while (true)
        {
            //공이 생성되는 간격으로 SetBall 실행. SetBall이 실행될 때 마다 ballNum 1씩 더해주기.
            yield return new WaitForSeconds(0.1f);
            SetBall();
            ballNum += 1;

            //ballNum이 45가 되면 코루틴 정지.
            if (ballNum >= 45)
            {
                StopCoroutine("StartMachine");

                //공 45개가 모두 생성이 되면 빌보드에 텍스트를 Ready로 바꾸기.
                billBoard.text = "Ready";
                ready = true;

                //공이 45개 다 생성되면 sound_SetBall 종료.
                sound_SetBall.SetActive(false);

                //공 45개가 다 생성되면 sound_Ready 실행.
                sound_Ready.SetActive(true);
            }
        }
    }

    //버튼을 누르면 추첨되도록 하는 버튼 기능.
    public void PressGetBtn()
    {
        //버튼을 누를 때 사운드 재생.
        sound_Btn.Play();

        //ready가 true고 공이 모두 생성된 후라면 추첨 코루틴 실행.
        if (ready)
        {
            //Get 버튼을 여러번 눌러도 한번만 실행.
            ready = false;

            StartCoroutine("Drow");

            //Get 버튼을 누르면 일단 billBoard 텍스트 비활성화.
            billBoard.gameObject.SetActive(false);

        }
    }

    //추천 6번 해줄 코루틴.
    IEnumerator Drow()
    {
        //당첨 List에 있는 숫자가 6개미만인 동안만 실행.
        while (winningNum.Count < 6)
        {
            //r에 있는 추천 기능을 실행.
            GetBall();
            yield return new WaitForSeconds(2);
        }
    }


    //추첨 기능.
    void GetBall()
    {
        //0부터 ball(44)중에 랜덤.
        int r = Random.Range(0, ball.Length);

        //r+1 숫자가 이미 당첨 List에 있다면 추첨 기능을 다시 실행.
        if (winningNum.Contains(r + 1))
        {
            GetBall();
            return;
        }

        //그렇게 뽑힌 r에 1을 더한 숫자가 당첨 List안에 없다면 당첨 List에 r에 더하기 1을 한 숫자를 넣습니다.
        if (!winningNum.Contains(r + 1))
        {
            winningNum.Add(r + 1);

            //그리고 r번째에 공에 볼스크립트를 가지고 와서 더 이상 통통 튀지 못하게 모든 코루틴 정지.
            ball[r].GetComponent<Ball>().StopAllCoroutines();

            //r번째 Rigidbody를 가져와서 더 이상 움직이지 않게 vilocity값을 제로로 만듬.
            ball[r].GetComponent<Rigidbody>().velocity = Vector3.zero;

            //정지 시킨 공을 출구로 옮기기.
            //r번째 공의 포지션을 출구의 포지션으로 보내기.
            ball[r].transform.position = exit.position;

            //공이 당첨되서 나올 때.
            sound_GetBall.Play();

            //전광판 당첨 숫자 만큼 반복 실행.
            for (int i = 0; i < num.Length; i++)
            {
                //오브젝트가 활성화 되어 있지 않는 경우에만.
                if (!num[i].gameObject.activeSelf)
                {

                    //i번쨰 전광판 숫자의 텍스트는 r+1한 숫자
                    num[i].text = string.Format("{0}", r + 1);

                    //i번째 전광판 숫자 활성화.
                    num[i].gameObject.SetActive(true);
                    return;

                }

            }

        }
    }

    //reset 버튼.
    public void PressResetBtn()
    {
        //버튼을 누를 때 사운드 재생.
        sound_Btn.Play();

        //버튼을 눌렀을 때 Invoke로 지연 실행.
        Invoke("ReStartBtn", 0.5f);

    }

    //잠시 후 재시작 기능.
    void ReStartBtn()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
