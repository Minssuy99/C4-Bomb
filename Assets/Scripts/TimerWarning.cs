using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerWarning : MonoBehaviour
{

    //타이머 시간이 촉박할 떄 게이머에게 경고하는 기능 작성

    /*
     *GameMaanager.cs의 time을 public으로 변경(참조하기 위함)
     *AudioManager.cs의 audioSource를 public으로 변경(참조하기 위함)
     *이 스크립트는 Canvas TimeTxt에 붙입니다.
     */

    Text timeText;         
    bool runningCorutine;   
    Coroutine coroutine;    
    
    float a = 10f; // 시간 설정을 위해 임의로 넣었다.

    void Start()
    {
        timeText = GetComponent<Text>();   // Text 색 변경을 위해 컴포넌트 가져오기
    }


    void Update()
    {
        /*
         *게임이 끝나기 a초 전
         *코루틴이 실행중이 아니고
         *타임 스케일이 0이 아니여야한다.
         */
        if (GameManager.Instance.time <= a && runningCorutine == false && Time.timeScale != 0)
        {
            coroutine = StartCoroutine(Warning());  //  Warning을 시작해라
        }
        else if (Time.timeScale == 0)   // 게임이 멈춰버린다면 (게임오버)
        {
            if (coroutine != null)      // 수정 사유 : 코루틴이 없는데 밑의 코드가 실행될 경우 error +999가 발생한다. (물론 게임에는 아무 지장 없다)
                StopCoroutine(coroutine);
            timeText.color = new Color(1f, 0f, 0f, 1);      // Text 색 변경 (빨강색)
            AudioManager.Instance.audioSource.pitch = 1.0f;
            runningCorutine = false;

        }
    }

    //a초가 남았을 때 시간의 색깔이 점점 빨간색으로 변하기+ 음악 속도 빠르게 하기
    IEnumerator Warning()
    {
        runningCorutine = true; 
        float count = 255f;

        for (int i = 0; i < a; i++)
        {
            count = 255f - 255f * i / a;    // 식을 임의로 수정했습니다.

            timeText.color = new Color(1f, count / 255f, count / 255f, 1);//시간텍스트 빨개짐
            AudioManager.Instance.audioSource.pitch += 0.1f;//소리속도 빨라짐

            yield return new WaitForSecondsRealtime(1f);    // 현실 시간으로 1초 정지

        }

        AudioManager.Instance.audioSource.pitch = 1.0f;
        runningCorutine = false;

    }

    // 변경 이전의 코드는 5초를 상정하게 만들었음을 알 수 있다.
    // 재활용성을 위해 변수 a를 만들어서 바꿔보자.
    // 변수로 바꾼다면 원하는 초를 쉽게쉽게 바꿔줄 수 있고, 원한다면 public을 통해서 바꿔줄 수 있다.
}
