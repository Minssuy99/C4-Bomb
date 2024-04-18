using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;    //싱글톤화
    public AudioSource audioSource;
    public AudioClip clip;

    void Awake()
    { 
        if (Instance == null)   // Insatnce가 비었다면
        {
            Instance = this;    // 나를 Instance에 넣어주세요
            DontDestroyOnLoad(gameObject);  // 씬을 넘어가도 나를 파괴하지 말아주세요
        }
        else       // 비어있지 않다면
        {
            Destroy(this);  // 나를 파괴해주세요
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = this.clip;   // 오디오 소스 클립에 clip을 넣어주세요
        audioSource.Play();     // 오디오 소스를 플레이해주세요
                                // (AudioSource 설정에 Loop가 체크되어있다면 반복 재생된다.
    }

}
