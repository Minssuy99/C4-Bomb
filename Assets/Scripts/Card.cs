using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int idx = 0;

    public string cardName;  // ��Ī �� �̸� ǥ��

    public SpriteRenderer frontImage; // front�� �̹��� �ֱ�

    public GameObject front;
    public GameObject back;

    Coroutine cardTimer;

    public Animator anim;

    // ��ġ �ִϸ��̼� ���� �ڵ�
    public int showNum;

    // �ߺ� ī�� ���� ���� �ڵ�
    bool openCard;

    AudioSource audioSource;  // ����� ����
    public AudioClip flip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();  // ����� ����

        Invoke("ShowCard", 0.1f * showNum);  // ��ġ �ִϸ��̼� ���� ����
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Setting(int number)
    {
        idx = number; // Board ��ũ��Ʈ���� number�� �Ѿ�� ���� idx�� ��ȯ
        frontImage.sprite = Resources.Load<Sprite>($"sparta{idx}"); // ���ڿ� �ֵ���ǥ �տ� $�� ���̸� {} �ȿ� ���� ��� ����
        // Resources ���� �ȿ� sparta0~9���� �̹��� ����, 0~5�� ���� / 6~9�� ��ź��

        SetCardName();
    }

    public void OpenCard()
    {
        if (GameManager.instance.secondCard != null || openCard || Time.timeScale == 0.0f)
            return;

        openCard = true;          // ������ ī�� ������ ����
        audioSource.PlayOneShot(flip);  // ������� ��ġ�� �ʰ� 1ȸ�� ����

        anim.SetBool("isOpen", true);
        // front.SetActive(true);
        // back.SetActive(false);  // �⺻ �ִϸ��̼�

        // firstCard�� ����ٸ�: firstCard�� �� ������ �Ѱ���
        if (GameManager.instance.firstCard == null)
        {
            GameManager.instance.firstCard = this;
            cardTimer = StartCoroutine(FirstCardTimer());  // ù ī�带 ������ 5�ʰ� ������ ����
        }
        // firstCard�� ������� �ʴٸ�: secondCard�� �� ������ �Ѱ��ְ�, Matched �Լ��� ȣ��
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
        // back.SetActive(true);  // �ִϸ����Ϳ� �ñ�
    }

    IEnumerator FirstCardTimer()  // ù ī�带 ������ 5�ʰ� ������ ����
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
                cardName = "������";
                break;
            case 1:
                cardName = "����";
                break;
            case 2:
                cardName = "��μ�";
                break;
            case 3:
                cardName = "������";
                break;
            case 4:
                cardName = "�����";
                break;
            case 5:
                cardName = "������";
                break;
            default:
                cardName = "��ź��";
                break;
        }
    }

    // ��ġ �ִϸ��̼� ���� �ڵ�
    void ShowCard()
    {
        anim.SetBool("ShowCard", true);     // �������� �ִϸ��̼� �ߵ�
        Invoke("ShowCardInvoke", 0.2f);     // ���� �ִϸ��̼� ����
    }
    void ShowCardInvoke()
    {
        anim.SetBool("ShowCard", false);    // CardIdle ���·� ����
        Invoke("CheckLastCard", 0.5f);      // Ÿ�̸� ������ �����ְ� ����
    }
    void CheckLastCard()
    {
        int lastCard = ((GameManager.sceneVariable.level + 2) * 4) - 1;
        if (showNum == lastCard)    // ������ ī����� �� ���
            GameManager.instance.timerOn = true;   // Ÿ�̸� �۵�
    }
}
