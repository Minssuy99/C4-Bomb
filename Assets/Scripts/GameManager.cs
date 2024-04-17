using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 이미 작성된 코드
    public Text timeTxt;        // 타이머 텍스트
    public GameObject endTxt;   // 끝 텍스트
        // 카드 정보 받아줄 변수들    
    public Card firstCard;
    public Card secondCard;

    public int cardCount = 0;   // 남은 카드 갯수
    public float time = 30.0f;  // 시간 변수

    // 시간 감소 표시 UI
    public GameObject penaltyTxt; 

    // 이름 표시 UI
    public Text nameText;   

    // 음악 담당
    AudioSource audioSource;
    public AudioClip clip;  // 성공 시 소리 음악파일
    public AudioClip clip2; // 실패 시 소리 음악파일 



    // 게임 종료 관련 변수
    public GameObject endPanel;     // 게임 종료 패널
    public Text matchNum;           // 매치 표시 텍스트
    public Text scoreTxt;           // 점수 표시 텍스트
    public Text timeLeftTxt;        // 게임 종료 후 남은 시간 텍스트
    public int cardMaxCount = 0;    // 이건 맞춘 카드 점수 계산을 위한 것 (총 카드 갯수)
    int matchCount = 0;             // 매칭 횟수 카운트
    int timeBonus = 100;            // 타임 보너스 점수
    int matchBonus = 100;           // 매치 보너스 점수
    int matchPenalty = 10;          // 매칭 페널티


    public GameObject bestTimePanel;     // 비활성화 전용
    public Text bestTimeTxt;        // 최고 시간 표시용

    string stageBestTime = "bestTime";

    public static class sceneVariable
    {
        public static int level = 1;  // 레벨
    }
    private void Awake()   
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        Time.timeScale = 1.0f;
        audioSource = GetComponent<AudioSource>(); // 오디오 컴포넌트 가져오기
        stageBestTime = sceneVariable.level + stageBestTime;     // 스테이지에 따른 변수 변경
        ShowBestTime();    // 최단 시간 보여주기 
    }

    void Update()
    {
        time -= Time.deltaTime;
        timeTxt.text = time.ToString("N2");
        if (time <= 0) 
        {
            time = 0;       // 혹여나 감소되서 음수가 되었을 때를 대비
            GameOver();     // 게임오버 호출
        }
    }
    void ShowBestTime()
    {
        if (PlayerPrefs.HasKey(stageBestTime))
        {
            bestTimeTxt.text = PlayerPrefs.GetFloat(stageBestTime).ToString("N2");
        }
        else
        {
            bestTimePanel.SetActive(false);
        }
    }
    void BestTime()
    {
        if (PlayerPrefs.HasKey(stageBestTime))
        {
            float bestTime = PlayerPrefs.GetFloat(stageBestTime);
            if (time > bestTime) { }
            else { return; }
        }
        PlayerPrefs.SetFloat(stageBestTime, time);
    }
    public void Matched()
    {
        
        if (firstCard.idx == secondCard.idx)    //카드가 같다면
        {
            audioSource.PlayOneShot(clip);  //정답 소리
            ShowName();                     //이름 보여주기

            MoveCard(firstCard);
            secondCard.gameObject.SetActive(false);


            // 맞춘 카드 파괴
            firstCard.DestroyCard();
            secondCard.DestroyCard();       
            cardCount -= 2;     //맞춰서 사라졋으니 남은 카드 감소

            if (cardCount == 0) // 더 이상 카드가 없다면
            {
                BestTime();     // 게임을 해냈으니 최단 시간 할당하는 함수 호출
                GameOver();     // 게임오버 함수 호출
            }
        }
        else        // 카드가 다르다면
        {
            // 다시 뒤집어놓기
            firstCard.CloseCard();
            secondCard.CloseCard();

            // 소리 파트
            audioSource.PlayOneShot(clip2); // 실패 소리

            // 시간 감소 코드 관련
            penaltyTxt.SetActive(true);     // 타이머 시간 감소 UI 표시
            time -= 0.5f;                   // 매칭 페널티
            Invoke("closeTxt", 0.5f);       // 타이머 시간 감소 UI 닫기

            // 카드색 변경
            Invoke("ChangeColor", 0.5f);    // 이미 건드린 카드 색깔 변경
        }
        matchCount++;           // 매칭 횟수 증가
        Invoke("CleanCard", 0.5f);// 카드 정보 삭제 함수 호출
    }
    void GameOver()         // 조건이 게임오버에 해당한다면 나를 불러주세요
    {
        Time.timeScale = 0.0f;
        // 결과창 반영
        matchNum.text = "매칭 시도 횟수 : " + matchCount.ToString();  //텍스트 반영
        timeLeftTxt.text = "남은 시간 : " + time.ToString("N2");        // 남은 시간 반영
        ScoreCalculate();
        endPanel.SetActive(true);
    }
    void CleanCard()    // 카드 정보 삭제
    {
        firstCard = null;
        secondCard = null;
    }

    void ScoreCalculate()   // 점수 계산
    {
        int timeScore = (int)time * timeBonus;
        int matchScore = (matchBonus * (cardMaxCount - cardCount)) - (matchCount * matchPenalty);
        // (매치 보너스 * (총 카드 갯수 - 남은 카드 갯수)) - (매치 횟수 * 매치 페널티)
        if (matchScore < 0 || timeScore < 0) { matchScore = 0; timeScore = 0; } // 음수 처리
        int score = timeScore + matchScore;
        scoreTxt.text = score.ToString();   // 점수 텍스트에 반영
    }

    void closeTxt()     // 페널티 텍스트 비활성화 함수
    {
        penaltyTxt.SetActive(false);
    }

    void ShowName()     // 이름 보여주기 함수
    {
        nameText.text = secondCard.cardName;
        Invoke("EraseName", 0.75f);
    }

    void EraseName()    // 이름 지우기 함수
    {
        nameText.text = null;
    }
   void MoveCard(Card card)
    {
        card.transform.position= new Vector2(-1.5f,4f);
        card.anim.SetBool("isMatch", true);
    }
    void ChangeColor()  // 색깔 바꾸기 함수
    {
        SpriteRenderer firstCardBackRenderer = firstCard.transform.Find("Back").GetComponent<SpriteRenderer>();
        SpriteRenderer secondCardBackRenderer = secondCard.transform.Find("Back").GetComponent<SpriteRenderer>(); // 태형님 파트
        Color greyColor = new Color(0.5f, 0.5f, 0.5f);
        firstCardBackRenderer.color = greyColor;
        secondCardBackRenderer.color = greyColor;
    }


}
