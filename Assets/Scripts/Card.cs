using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    // 이미 적힌 코드
    public int idx = 0;
    public GameObject front;
    public GameObject back;

    //그래픽 담당
    public Animator anim;

    // 음악 담당
    AudioSource audioSource;
    public AudioClip clip;

    public SpriteRenderer frontImage;   // 앞면 이미지를 담을 변수

    // 카드 이름 정보 담는 변수
    public string cardName;

    // 중복 카드 열기 방지 코드
    bool openCard;

    //
    Coroutine CardTimer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();  //오디오 소스 컴포넌트를 건들겠다
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Setting(int number)
    {
        idx = number;
        frontImage.sprite = Resources.Load<Sprite>($"team{idx}");   // 파일에서 이미지 가져오기

        SetCardName();
    }
    public void OpenCard()
    {
        // 이건 두번째 카드 매칭 중일 때나 이미 열린 카드를 열려고 할 때 방지하는 코드
        // 끝 화면에서 뒤에가려진 카드를 맞출경우 점수가 올라가는거 방지
        if (GameManager.Instance.secondCard != null || openCard == true || Time.timeScale == 0)
            return;     // 함수에서 나간다.

        openCard = true;          // 동일한 카드 누르기 방지
        audioSource.PlayOneShot(clip);  // 뒤집는 소리

        anim.SetBool("isOpen", true);   // 애니메이션 발동

        //front.SetActive(true);
        //back.SetActive(false); (이건 예전 애니메이션)

        // 카드 딜레이 주기 및 카드 오픈 다시 활성화 (안 넣으면 똑같은 걸 광클하면 맞춰진다;)
        Invoke("OpenDelay", 0.25f);

        // 카드 정보를 넘기는 조건문
        if (GameManager.Instance.firstCard == null)
        {
            //5초 카운트
            CardTimer = StartCoroutine(FirstCardTimer());
            GameManager.Instance.firstCard = this;
        }
        else
        {
            GameManager.Instance.secondCard = this;
            GameManager.Instance.Matched();

        }

    }

    // 딜레이용 함수
    void OpenDelay()
    {
        openCard = false;
    }


    // 카드 처리 함수들
    // Invoke를 넣은 이유는 공개와 파괴가 지연되어야 코드가 안 꼬일 것이라고 판단
    public void DestroyCard()
    {
        //transform.position= new Vector2(-1.5f,4f);
        // anim.SetBool("isMatch", true);   // 애니메이션 발동
        Invoke("DestroyCardInvoke", 3f);
    }

    void DestroyCardInvoke()
    {
        Destroy(gameObject);
    }
    public void CloseCard()
    {
        if (CardTimer != null)
            StopCoroutine(CardTimer);
        Invoke("CloseCardInvoke", 1f);
    }
    void CloseCardInvoke()
    {
        anim.SetBool("isOpen", false);
        back.SetActive(true);
        front.SetActive(false);
    }

    private void SetCardName()      // 이름 정보 기입
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
                cardName = "백흰범";
                break;
            case 4:
                cardName = "양은수";
                break;
            case 5:
                cardName = "김태형";
                break;
            default:
                cardName = "르탄이";   // 임시로 수정했습니다.
                break;
        }
    }

    IEnumerator FirstCardTimer()
    {
        yield return new WaitForSeconds(5f);
        GameManager.Instance.firstCard = null;
        CloseCard();

    }

}