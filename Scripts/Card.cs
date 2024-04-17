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

    AudioSource audioSource;  // 오디오 삽입
    public AudioClip flip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();  // 오디오 삽입
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
        if (GameManager.instance.secondCard != null || Time.timeScale == 0.0f) return;

        audioSource.PlayOneShot(flip);  // 오디오가 겹치지 않게 1회만 실행

        anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);

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
        Invoke("DestroyCardInvoke", 0.5f);
    }

    void DestroyCardInvoke()
    {
        Destroy(gameObject);
    }

    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 0.5f);
    }

    void CloseCardInvoke()
    {
        if (cardTimer != null)
        {
            StopCoroutine(cardTimer);
        }

        anim.SetBool("isOpen", false);
        front.SetActive(false);
        back.SetActive(true);
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
}
