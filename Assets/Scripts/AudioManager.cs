using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;    //�̱���ȭ
    public AudioSource audioSource;
    public AudioClip clip;

    void Awake()
    { 
        if (Instance == null)   // Insatnce�� ����ٸ�
        {
            Instance = this;    // ���� Instance�� �־��ּ���
            DontDestroyOnLoad(gameObject);  // ���� �Ѿ�� ���� �ı����� �����ּ���
        }
        else       // ������� �ʴٸ�
        {
            Destroy(this);  // ���� �ı����ּ���
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = this.clip;   // ����� �ҽ� Ŭ���� clip�� �־��ּ���
        audioSource.Play();     // ����� �ҽ��� �÷������ּ���
                                // (AudioSource ������ Loop�� üũ�Ǿ��ִٸ� �ݺ� ����ȴ�.
    }

}
