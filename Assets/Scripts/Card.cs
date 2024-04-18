using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int idx = 0;

    public string cardName;  // 매칭 시 이름 표시

    public SpriteRenderer frontImage; // front에 이미지 넣기

    public GameObject front;
    public GameObject back;

    Coroutine cardTimer;

    public Animator anim;

    // 배치 애니메이션 전용 코드
    public int showNum;

    // 중복 카드 열기 방지 코드
    bool openCard;

    AudioSource audioSource;  // 오디오 삽입
    public AudioClip flip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();  // 오디오 삽입

        Invoke("ShowCard", 0.1f * showNum);  // 배치 애니메이션 순차 실행
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Setting(int number)
    {
        idx = number; // Board 스크립트에서 number로 넘어온 값을 idx로 변환
        frontImage.sprite = Resources.Load<Sprite>($"sparta{idx}"); // 문자열 쌍따옴표 앞에 $를 붙이면 {} 안에 변수 사용 가능
        // Resources 폴더 안에 sparta0~9까지 이미지 삽입, 0~5는 팀원 / 6~9는 르탄이

        SetCardName();
    }

    public void OpenCard()
    {
        if (GameManager.instance.secondCard != null || openCard || Time.timeScale == 0.0f)
            return;

        openCard = true;          // 동일한 카드 누르기 방지
        audioSource.PlayOneShot(flip);  // 오디오가 겹치지 않게 1회만 실행

        anim.SetBool("isOpen", true);
        // front.SetActive(true);
        // back.SetActive(false);  // 기본 애니메이션

        // firstCard가 비었다면: firstCard에 내 정보를 넘겨줌
        if (GameManager.instance.firstCard == null)
        {
            GameManager.instance.firstCard = this;
            cardTimer = StartCoroutine(FirstCardTimer());  // 첫 카드를 뒤집고 5초가 지나면 리셋
        }
        // firstCard가 비어있지 않다면: secondCard에 내 정보를 넘겨주고, Matched 함수를 호출
        else
        {
            GameManager.instance.secondCard = this;
            GameManager.instance.Matched();
        }
    }

    public void DestroyCard()
    {
        anim.SetBool("isMatch", true);
        Invoke("DestroyCardInvoke", 3f);
    }

    void DestroyCardInvoke()
    {
        Destroy(gameObject);
    }

    public void CloseCard()
    {
        if (cardTimer != null)
            StopCoroutine(cardTimer);

        openCard = false;
        Invoke("CloseCardInvoke", 0.5f);
    }

    void CloseCardInvoke()
    {
        anim.SetBool("isOpen", false);
        // front.SetActive(false);
        // back.SetActive(true);  // 애니메이터에 맡김
    }

    IEnumerator FirstCardTimer()  // 첫 카드를 뒤집고 5초가 지나면 리셋
    {
        yield return new WaitForSeconds(5f);
        GameManager.instance.firstCard = null;
        CloseCard();
    }

    void SetCardName()
    {
        switch (idx)
        {
            case 0:
                cardName = "강수지";
                break;
            case 1:
                cardName = "고도희";
                break;
            case 2:
                cardName = "김민성";
                break;
            case 3:
                cardName = "김태형";
                break;
            case 4:
                cardName = "백흰범";
                break;
            case 5:
                cardName = "양은수";
                break;
            default:
                cardName = "르탄이";
                break;
        }
    }

    // 배치 애니메이션 전용 코드
    void ShowCard()
    {
        anim.SetBool("ShowCard", true);     // 떨어지는 애니메이션 발동
        Invoke("ShowCardInvoke", 0.2f);     // 다음 애니메이션 지연
    }
    void ShowCardInvoke()
    {
        anim.SetBool("ShowCard", false);    // CardIdle 상태로 진입
        Invoke("CheckLastCard", 0.5f);      // 타이머 실행을 여유있게 설정
    }
    void CheckLastCard()
    {
        int lastCard = ((GameManager.sceneVariable.level + 2) * 4) - 1;
        if (showNum == lastCard)    // 마지막 카드까지 깔릴 경우
            GameManager.instance.timerOn = true;   // 타이머 작동
    }
}
