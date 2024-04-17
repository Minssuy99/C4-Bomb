using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerWarning : MonoBehaviour
{

    //Ÿ�̸� �ð��� �˹��� �� ���̸ӿ��� ����ϴ� ��� �ۼ�

    /*
     *GameMaanager.cs�� time�� public���� ����(�����ϱ� ����)
     *AudioManager.cs�� audioSource�� public���� ����(�����ϱ� ����)
     *�� ��ũ��Ʈ�� Canvas TimeTxt�� ���Դϴ�.
     */

    Text timeText;         
    bool runningCorutine;   
    Coroutine coroutine;    
    
    float a = 10f; // �ð� ������ ���� ���Ƿ� �־���.

    void Start()
    {
        timeText = GetComponent<Text>();   // Text �� ������ ���� ������Ʈ ��������
    }


    void Update()
    {
        /*
         *������ ������ a�� ��
         *�ڷ�ƾ�� �������� �ƴϰ�
         *Ÿ�� �������� 0�� �ƴϿ����Ѵ�.
         */
        if (GameManager.Instance.time <= a && runningCorutine == false && Time.timeScale != 0)
        {
            coroutine = StartCoroutine(Warning());  //  Warning�� �����ض�
        }
        else if (Time.timeScale == 0)   // ������ ��������ٸ� (���ӿ���)
        {
            if (coroutine != null)      // ���� ���� : �ڷ�ƾ�� ���µ� ���� �ڵ尡 ����� ��� error +999�� �߻��Ѵ�. (���� ���ӿ��� �ƹ� ���� ����)
                StopCoroutine(coroutine);
            timeText.color = new Color(1f, 0f, 0f, 1);      // Text �� ���� (������)
            AudioManager.Instance.audioSource.pitch = 1.0f;
            runningCorutine = false;

        }
    }

    //a�ʰ� ������ �� �ð��� ������ ���� ���������� ���ϱ�+ ���� �ӵ� ������ �ϱ�
    IEnumerator Warning()
    {
        runningCorutine = true; 
        float count = 255f;

        for (int i = 0; i < a; i++)
        {
            count = 255f - 255f * i / a;    // ���� ���Ƿ� �����߽��ϴ�.

            timeText.color = new Color(1f, count / 255f, count / 255f, 1);//�ð��ؽ�Ʈ ������
            AudioManager.Instance.audioSource.pitch += 0.1f;//�Ҹ��ӵ� ������

            yield return new WaitForSecondsRealtime(1f);    // ���� �ð����� 1�� ����

        }

        AudioManager.Instance.audioSource.pitch = 1.0f;
        runningCorutine = false;

    }

    // ���� ������ �ڵ�� 5�ʸ� �����ϰ� ��������� �� �� �ִ�.
    // ��Ȱ�뼺�� ���� ���� a�� ���� �ٲ㺸��.
    // ������ �ٲ۴ٸ� ���ϴ� �ʸ� ���Խ��� �ٲ��� �� �ְ�, ���Ѵٸ� public�� ���ؼ� �ٲ��� �� �ִ�.
}
