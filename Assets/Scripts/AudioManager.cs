using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;  // ����� ����
    public AudioClip clip;

    public void Awake()  // �̱���
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // ���� �̵��ص� AudioManager ���� ������Ʈ�� �ı����� ����
        }
        else
        {
            Destroy(gameObject);  // �̱����� 2�� �̻��� �� �� �ϳ��� �ı� ó��
        }

        Screen.SetResolution(720, 1280, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.Play();
    }
}
