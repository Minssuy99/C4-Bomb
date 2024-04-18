using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    // �̹� ���� �ڵ�
    public int idx = 0;
    public GameObject front;
    public GameObject back;

        //�׷��� ���
    public Animator anim;

    // ��ġ �ִϸ��̼� ���� �ڵ�
    public int showNum;

    // ���� ���
    AudioSource audioSource;
    public AudioClip clip;

    public SpriteRenderer frontImage;   // �ո� �̹����� ���� ����

    // ī�� �̸� ���� ��� ����
    public string cardName;

    // �ߺ� ī�� ���� ���� �ڵ�
    bool openCard;

    //
    Coroutine CardTimer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();  //����� �ҽ� ������Ʈ�� �ǵ�ڴ�
        Invoke("ShowCard", 0.1f * showNum); // ��ġ �ִϸ��̼� ���� ����
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setting(int number)
    {
        idx = number;
        frontImage.sprite = Resources.Load<Sprite>($"team{idx}");   // ���Ͽ��� �̹��� ��������

        SetCardName();
    }
    public void OpenCard()
    {
        // �̰� �ι�° ī�� ��Ī ���� ���� �̹� ���� ī�带 ������ �� �� �����ϴ� �ڵ�
        // �� ȭ�鿡�� �ڿ������� ī�带 ������ ������ �ö󰡴°� ����
        // ī�尡 ��ġ�ǰ� ���� �� ������ �� ����
        if (GameManager.Instance.secondCard != null || openCard ||Time.timeScale==0 || !GameManager.Instance.timerOn)    
            return;     // �Լ����� ������.

        openCard = true;          // ������ ī�� ������ ����
        audioSource.PlayOneShot(clip);  // ������ �Ҹ�

        anim.SetBool("isOpen", true);   // �ִϸ��̼� �ߵ�

        //front.SetActive(true);
        //back.SetActive(false); (�̰� ���� �ִϸ��̼�)

        // ī�� ������ �ֱ� �� ī�� ���� �ٽ� Ȱ��ȭ (�� ������ �Ȱ��� �� ��Ŭ�ϸ� ��������;)
        //Invoke("OpenDelay", 0.25f);

        // ī�� ������ �ѱ�� ���ǹ�
        if (GameManager.Instance.firstCard == null)
        {
            //5�� ī��Ʈ
            CardTimer = StartCoroutine(FirstCardTimer());
            GameManager.Instance.firstCard = this;
        }
        else
        {
            GameManager.Instance.secondCard = this;
            GameManager.Instance.Matched();
            
        }

    }

    /*
    // �����̿� �Լ�
    void OpenDelay() 
    {
        openCard = false;
    } 
    */

    // ī�� ó�� �Լ���
    // Invoke�� ���� ������ ������ �ı��� �����Ǿ�� �ڵ尡 �� ���� ���̶�� �Ǵ�
    public void DestroyCard()
    {
        //transform.position= new Vector2(-1.5f,4f);
        anim.SetBool("isMatch", true);
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
        //back.SetActive(true);
        //front.SetActive(false);   // �ִϸ����Ͱ� �� �˾Ƽ� ���ٰž�.
        openCard = false;
    }

    private void SetCardName()      // �̸� ���� ����
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
                cardName = "�����";
                break;
            case 4:
                cardName = "������";
                break;
            case 5:
                cardName = "������";
                break;
            default:
                cardName = "��ź��";   // �ӽ÷� �����߽��ϴ�.
                break;
        }
    }

   IEnumerator FirstCardTimer()
   {
        yield return new WaitForSeconds(5f);
        GameManager.Instance.firstCard = null;
        CloseCard();
      
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
            GameManager.Instance.timerOn = true;   // Ÿ�̸� �۵�
    }

}