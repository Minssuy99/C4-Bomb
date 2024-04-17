using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // �̹� �ۼ��� �ڵ�
    public Text timeTxt;        // Ÿ�̸� �ؽ�Ʈ
    public GameObject endTxt;   // �� �ؽ�Ʈ
        // ī�� ���� �޾��� ������    
    public Card firstCard;
    public Card secondCard;

    public int cardCount = 0;   // ���� ī�� ����
    public float time = 30.0f;  // �ð� ����

    // �ð� ���� ǥ�� UI
    public GameObject penaltyTxt; 

    // �̸� ǥ�� UI
    public Text nameText;   

    // ���� ���
    AudioSource audioSource;
    public AudioClip clip;  // ���� �� �Ҹ� ��������
    public AudioClip clip2; // ���� �� �Ҹ� �������� 



    // ���� ���� ���� ����
    public GameObject endPanel;     // ���� ���� �г�
    public Text matchNum;           // ��ġ ǥ�� �ؽ�Ʈ
    public Text scoreTxt;           // ���� ǥ�� �ؽ�Ʈ
    public Text timeLeftTxt;        // ���� ���� �� ���� �ð� �ؽ�Ʈ
    public int cardMaxCount = 0;    // �̰� ���� ī�� ���� ����� ���� �� (�� ī�� ����)
    int matchCount = 0;             // ��Ī Ƚ�� ī��Ʈ
    int timeBonus = 100;            // Ÿ�� ���ʽ� ����
    int matchBonus = 100;           // ��ġ ���ʽ� ����
    int matchPenalty = 10;          // ��Ī ���Ƽ


    public GameObject bestTimePanel;     // ��Ȱ��ȭ ����
    public Text bestTimeTxt;        // �ְ� �ð� ǥ�ÿ�

    string stageBestTime = "bestTime";

    public static class sceneVariable
    {
        public static int level = 1;  // ����
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
        audioSource = GetComponent<AudioSource>(); // ����� ������Ʈ ��������
        stageBestTime = sceneVariable.level + stageBestTime;     // ���������� ���� ���� ����
        ShowBestTime();    // �ִ� �ð� �����ֱ� 
    }

    void Update()
    {
        time -= Time.deltaTime;
        timeTxt.text = time.ToString("N2");
        if (time <= 0) 
        {
            time = 0;       // Ȥ���� ���ҵǼ� ������ �Ǿ��� ���� ���
            GameOver();     // ���ӿ��� ȣ��
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
        
        if (firstCard.idx == secondCard.idx)    //ī�尡 ���ٸ�
        {
            audioSource.PlayOneShot(clip);  //���� �Ҹ�
            ShowName();                     //�̸� �����ֱ�

            MoveCard(firstCard);
            secondCard.gameObject.SetActive(false);


            // ���� ī�� �ı�
            firstCard.DestroyCard();
            secondCard.DestroyCard();       
            cardCount -= 2;     //���缭 ������� ���� ī�� ����

            if (cardCount == 0) // �� �̻� ī�尡 ���ٸ�
            {
                BestTime();     // ������ �س����� �ִ� �ð� �Ҵ��ϴ� �Լ� ȣ��
                GameOver();     // ���ӿ��� �Լ� ȣ��
            }
        }
        else        // ī�尡 �ٸ��ٸ�
        {
            // �ٽ� ���������
            firstCard.CloseCard();
            secondCard.CloseCard();

            // �Ҹ� ��Ʈ
            audioSource.PlayOneShot(clip2); // ���� �Ҹ�

            // �ð� ���� �ڵ� ����
            penaltyTxt.SetActive(true);     // Ÿ�̸� �ð� ���� UI ǥ��
            time -= 0.5f;                   // ��Ī ���Ƽ
            Invoke("closeTxt", 0.5f);       // Ÿ�̸� �ð� ���� UI �ݱ�

            // ī��� ����
            Invoke("ChangeColor", 0.5f);    // �̹� �ǵ帰 ī�� ���� ����
        }
        matchCount++;           // ��Ī Ƚ�� ����
        Invoke("CleanCard", 0.5f);// ī�� ���� ���� �Լ� ȣ��
    }
    void GameOver()         // ������ ���ӿ����� �ش��Ѵٸ� ���� �ҷ��ּ���
    {
        Time.timeScale = 0.0f;
        // ���â �ݿ�
        matchNum.text = "��Ī �õ� Ƚ�� : " + matchCount.ToString();  //�ؽ�Ʈ �ݿ�
        timeLeftTxt.text = "���� �ð� : " + time.ToString("N2");        // ���� �ð� �ݿ�
        ScoreCalculate();
        endPanel.SetActive(true);
    }
    void CleanCard()    // ī�� ���� ����
    {
        firstCard = null;
        secondCard = null;
    }

    void ScoreCalculate()   // ���� ���
    {
        int timeScore = (int)time * timeBonus;
        int matchScore = (matchBonus * (cardMaxCount - cardCount)) - (matchCount * matchPenalty);
        // (��ġ ���ʽ� * (�� ī�� ���� - ���� ī�� ����)) - (��ġ Ƚ�� * ��ġ ���Ƽ)
        if (matchScore < 0 || timeScore < 0) { matchScore = 0; timeScore = 0; } // ���� ó��
        int score = timeScore + matchScore;
        scoreTxt.text = score.ToString();   // ���� �ؽ�Ʈ�� �ݿ�
    }

    void closeTxt()     // ���Ƽ �ؽ�Ʈ ��Ȱ��ȭ �Լ�
    {
        penaltyTxt.SetActive(false);
    }

    void ShowName()     // �̸� �����ֱ� �Լ�
    {
        nameText.text = secondCard.cardName;
        Invoke("EraseName", 0.75f);
    }

    void EraseName()    // �̸� ����� �Լ�
    {
        nameText.text = null;
    }
   void MoveCard(Card card)
    {
        card.transform.position= new Vector2(-1.5f,4f);
        card.anim.SetBool("isMatch", true);
    }
    void ChangeColor()  // ���� �ٲٱ� �Լ�
    {
        SpriteRenderer firstCardBackRenderer = firstCard.transform.Find("Back").GetComponent<SpriteRenderer>();
        SpriteRenderer secondCardBackRenderer = secondCard.transform.Find("Back").GetComponent<SpriteRenderer>(); // ������ ��Ʈ
        Color greyColor = new Color(0.5f, 0.5f, 0.5f);
        firstCardBackRenderer.color = greyColor;
        secondCardBackRenderer.color = greyColor;
    }


}
