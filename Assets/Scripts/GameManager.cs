using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Ÿ�̸�

public class GameManager : MonoBehaviour
{
    public static GameManager instance;  // �̱���
    
    public Card firstCard;
    public Card secondCard;

    public Text timeTxt;  // Ÿ�̸�
    public float time = 30.0f;
    Color red = new Color32(255, 0, 0, 255);
    bool noTime = false;  // if (time <= 5.0f) ���� �� ���� ����ǰ�
    bool gameOn = true;

    public Text successTxt;
    public GameObject successPanel;
    public GameObject failPanel;  // ī�带 ������ �� �̸� Ȥ�� ��! �� ǥ�õǰ�

    public Text penaltyTxt;  // ������ ������ �ð� ����

    public int matchCount;  // ��Ī Ƚ��
    public int cardMaxCount = 0;    // �ִ� ī�� ���� (���� ���� ������ �ݿ��� ���̴�.)
    int timeBonus = 100;        // �ð� ������ 'time * timeBonus'�� ����� ����
    int matchBonus = 100;    // ī�带 ���߸� ��� ������ 'matchBonus * ���� ��'�� ��ȹ
    int matchPenalty = 10;     // ��Ī�� Ƚ����ŭ ������ ��´�. 'matchCount * matchPenalty'

    public GameObject endPanel;
    public Text timeLeftTxt;
    public Text matchTxt;
    public Text scoreTxt;
    public int cardCount = 0;  // ���� ����

    public static class sceneVariable
    {
        public static int level = 1;  // ����
        public static int openStage = 1;  // �رݵ� ��������
    }

    public GameObject bestTimePanel;     // ��Ȱ��ȭ ����
    public Text bestTimeTxt;        // �ְ� �ð� ǥ�ÿ�
    string stageBestTime = "bestTime";  // ������ �ְ� �ð� ǥ�ÿ�

    AudioSource audioSource;  // ����� ����
    public AudioClip success;
    public AudioClip fail;

    public void Awake()  // �̱���
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;  // �ִ� ������ ����

        Time.timeScale = 1.0f;  // ���� ���� �� ����� �� ���� ������

        AudioManager am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        am.audioSource.pitch = 1.0f;

        audioSource = GetComponent<AudioSource>();  // ����� ����

        stageBestTime = sceneVariable.level + stageBestTime;     // ���߿� PlayerPrefs�� ������ �� ���
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

        if (time <= 7.0f && !noTime)  // �ð��� 7�� ���Ϸ� ������ �� Ÿ�̸� ���� ���ڷ�, bgm ��ġ �ø���
        {
            noTime = true;

            timeTxt.color = red;

            AudioManager am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            am.audioSource.pitch = 1.2f;
        }

        if (time <= 0.0f)  // ���� ����
        {
            time = 0.0f;
            GameOver();
        }

        timeTxt.text = time.ToString("N2"); // Ÿ�̸�, �Ҽ��� ��° �ڸ�����
    }

    public void Matched()
    {
        matchCount++;

        SpriteRenderer firstCardBackRenderer = firstCard.transform.Find("Back").GetComponent<SpriteRenderer>();
        SpriteRenderer secondCardBackRenderer = secondCard.transform.Find("Back").GetComponent<SpriteRenderer>();

        if (firstCard.idx == secondCard.idx)
        {
            cardCount -= 2;

            successTxt.text = secondCard.cardName;  // ���� �� �̸� ǥ��
            successPanel.SetActive(true);
            Invoke("DestroySuccessPanel", 1.0f);  // �̸� ���� 1�ʵڿ� �����

            audioSource.PlayOneShot(success);  // ����� ����

            firstCard.DestroyCard();
            secondCard.DestroyCard();

            if (cardCount == 0)  // ���� Ŭ����
            {
                gameOn = false;
                BestTime();     // ������ �س����� �ִ� �ð� �Լ� ȣ��
                Invoke("GameOver", 0.6f);

                if (sceneVariable.level == 1 && sceneVariable.openStage == 1)
                {
                    sceneVariable.openStage = 2;
                }
                else if (sceneVariable.level == 2 && sceneVariable.openStage == 2)
                {
                    sceneVariable.openStage = 3;
                }  // ���� �������� Ŭ���� �� ���� �������� ����
            }
        }
        else
        {
            audioSource.PlayOneShot(fail);

            Color greyColor = new Color(0.75f, 0.75f, 0.75f);
            firstCardBackRenderer.color = greyColor;
            secondCardBackRenderer.color = greyColor;

            failPanel.SetActive(true);
            Invoke("DestroyFailPanel", 1.0f);  // ��! ���� 1�ʵڿ� �����

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
        if (PlayerPrefs.HasKey(stageBestTime))    // �ְ� �ð��� �ֳ���?
        {
            float bestTime = PlayerPrefs.GetFloat(stageBestTime); // �ְ� �ð��� ������ ��
            if (time > bestTime)        //���� �ð��� �ְ� �ð� ���� ������?
            {
                PlayerPrefs.SetFloat(stageBestTime, time);    // ����
            }
        }
        else
        {
            PlayerPrefs.SetFloat(stageBestTime, time);        // ����
        }
    }

    void ShowBestTime()
    {
        if (PlayerPrefs.HasKey(stageBestTime))
        {
            bestTimeTxt.text = PlayerPrefs.GetFloat(stageBestTime).ToString("N2");
        }
        else    // ������ �����
        {
            bestTimePanel.SetActive(false);
        }
    }

    void ScoreCalculate()
    {
        int timeScore = (int)time * timeBonus;
        int matchScore = (matchBonus * (cardMaxCount - cardCount)) - (matchCount * matchPenalty);
        // (��ġ���ʽ� * (�� ī�� ���� - ���� ī�� ����)) - (��Ī �õ� Ƚ�� * ��Ī ���Ƽ)

        if (matchScore < 0 || timeScore < 0)
        {
            matchScore = 0;
            timeScore = 0;
        } // ���� ó��
        
        int score = timeScore + matchScore;
        scoreTxt.text = "���� : " + score.ToString();
    }

    void GameOver()
    {
        noTime = true;

        AudioManager am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        am.audioSource.pitch = 1.0f;

        Time.timeScale = 0.0f;

        matchTxt.text = "��Ī �õ� Ƚ�� : " + matchCount.ToString();
        timeLeftTxt.text = "���� �ð� : " + time.ToString("N2");
        ScoreCalculate();

        endPanel.SetActive(true);
    }
}
