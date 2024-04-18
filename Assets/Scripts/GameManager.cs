using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // 타이머

public class GameManager : MonoBehaviour
{
    public static GameManager instance;  // 싱글턴
    
    public Card firstCard;
    public Card secondCard;

    public Text timeTxt;  // 타이머
    public float time = 30.0f;
    Color red = new Color32(255, 0, 0, 255);
    bool noTime = false;  // if (time <= 5.0f) 문이 한 번만 실행되게
    bool gameOn = true;

    public Text successTxt;
    public GameObject successPanel;
    public GameObject failPanel;  // 카드를 맞췄을 때 이름 혹은 땡! 이 표시되게

    public Text penaltyTxt;  // 실패할 때마다 시간 감소

    public int matchCount;  // 매칭 횟수
    public int cardMaxCount = 0;    // 최대 카드 갯수 (맞춘 수도 점수에 반영할 것이다.)
    int timeBonus = 100;        // 시간 점수로 'time * timeBonus'로 계산할 예정
    int matchBonus = 100;    // 카드를 맞추면 얻는 점수로 'matchBonus * 맞춘 수'로 계획
    int matchPenalty = 10;     // 매칭한 횟수만큼 점수를 깎는다. 'matchCount * matchPenalty'

    public GameObject endPanel;
    public Text timeLeftTxt;
    public Text matchTxt;
    public Text scoreTxt;
    public int cardCount = 0;  // 게임 오버

    public static class sceneVariable
    {
        public static int level = 1;  // 레벨
        public static int openStage = 1;  // 해금된 스테이지
    }

    public GameObject bestTimePanel;     // 비활성화 전용
    public Text bestTimeTxt;        // 최고 시간 표시용
    string stageBestTime = "bestTime";  // 레벨별 최고 시간 표시용

    AudioSource audioSource;  // 오디오 삽입
    public AudioClip success;
    public AudioClip fail;

    public void Awake()  // 싱글턴
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;  // 최대 프레임 고정

        Time.timeScale = 1.0f;  // 게임 오버 후 재시작 시 게임 재진행

        AudioManager am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        am.audioSource.pitch = 1.0f;

        audioSource = GetComponent<AudioSource>();  // 오디오 삽입

        stageBestTime = sceneVariable.level + stageBestTime;     // 나중에 PlayerPrefs를 구별할 때 사용
        ShowBestTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOn)
        {
            time -= Time.deltaTime;
        }
        else
        {
            PlayerPrefs.SetFloat("endTime", time);
            time = PlayerPrefs.GetFloat("endTime");
        }

        if (time <= 7.0f && !noTime)  // 시간이 7초 이하로 남았을 때 타이머 빨간 글자로, bgm 피치 올리기
        {
            noTime = true;

            timeTxt.color = red;

            AudioManager am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            am.audioSource.pitch = 1.2f;
        }

        if (time <= 0.0f)  // 게임 오버
        {
            time = 0.0f;
            GameOver();
        }

        timeTxt.text = time.ToString("N2"); // 타이머, 소수점 둘째 자리까지
    }

    public void Matched()
    {
        matchCount++;

        SpriteRenderer firstCardBackRenderer = firstCard.transform.Find("Back").GetComponent<SpriteRenderer>();
        SpriteRenderer secondCardBackRenderer = secondCard.transform.Find("Back").GetComponent<SpriteRenderer>();

        if (firstCard.idx == secondCard.idx)
        {
            cardCount -= 2;

            successTxt.text = secondCard.cardName;  // 성공 시 이름 표시
            successPanel.SetActive(true);
            Invoke("DestroySuccessPanel", 1.0f);  // 이름 문구 1초뒤에 사라짐

            audioSource.PlayOneShot(success);  // 오디오 삽입

            firstCard.DestroyCard();
            secondCard.DestroyCard();

            if (cardCount == 0)  // 게임 클리어
            {
                gameOn = false;
                BestTime();     // 게임을 해냈으니 최단 시간 함수 호출
                Invoke("GameOver", 0.6f);

                if (sceneVariable.level == 1 && sceneVariable.openStage == 1)
                {
                    sceneVariable.openStage = 2;
                }
                else if (sceneVariable.level == 2 && sceneVariable.openStage == 2)
                {
                    sceneVariable.openStage = 3;
                }  // 이전 스테이지 클리어 시 다음 스테이지 오픈
            }
        }
        else
        {
            audioSource.PlayOneShot(fail);

            Color greyColor = new Color(0.75f, 0.75f, 0.75f);
            firstCardBackRenderer.color = greyColor;
            secondCardBackRenderer.color = greyColor;

            failPanel.SetActive(true);
            Invoke("DestroyFailPanel", 1.0f);  // 땡! 문구 1초뒤에 사라짐

            penaltyTxt.gameObject.SetActive(true);
            time -= 0.5f;
            Invoke("CloseTxt", 1.0f);

            firstCard.CloseCard();
            secondCard.CloseCard();
        }
        
        firstCard = null;
        secondCard = null;
    }
    void CloseTxt()
    {
        penaltyTxt.gameObject.SetActive(false);
    }

    void DestroySuccessPanel()
    {
        successPanel.SetActive(false);
    }

    void DestroyFailPanel()
    {
        failPanel.SetActive(false);
    }

    void BestTime()
    {
        if (PlayerPrefs.HasKey(stageBestTime))    // 최고 시간이 있나요?
        {
            float bestTime = PlayerPrefs.GetFloat(stageBestTime); // 최고 시간을 가지고 옴
            if (time > bestTime)        //남은 시간이 최고 시간 보다 많나요?
            {
                PlayerPrefs.SetFloat(stageBestTime, time);    // 갱신
            }
        }
        else
        {
            PlayerPrefs.SetFloat(stageBestTime, time);        // 갱신
        }
    }

    void ShowBestTime()
    {
        if (PlayerPrefs.HasKey(stageBestTime))
        {
            bestTimeTxt.text = PlayerPrefs.GetFloat(stageBestTime).ToString("N2");
        }
        else    // 없으면 숨기기
        {
            bestTimePanel.SetActive(false);
        }
    }

    void ScoreCalculate()
    {
        int timeScore = (int)time * timeBonus;
        int matchScore = (matchBonus * (cardMaxCount - cardCount)) - (matchCount * matchPenalty);
        // (매치보너스 * (총 카드 갯수 - 남은 카드 갯수)) - (매칭 시도 횟수 * 매칭 페널티)

        if (matchScore < 0 || timeScore < 0)
        {
            matchScore = 0;
            timeScore = 0;
        } // 음수 처리
        
        int score = timeScore + matchScore;
        scoreTxt.text = "점수 : " + score.ToString();
    }

    void GameOver()
    {
        noTime = true;

        AudioManager am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        am.audioSource.pitch = 1.0f;

        Time.timeScale = 0.0f;

        matchTxt.text = "매칭 시도 횟수 : " + matchCount.ToString();
        timeLeftTxt.text = "남은 시간 : " + time.ToString("N2");
        ScoreCalculate();

        endPanel.SetActive(true);
    }
}
