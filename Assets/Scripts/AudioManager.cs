using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;  // 오디오 삽입
    public AudioClip clip;

    public void Awake()  // 싱글턴
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // 씬을 이동해도 AudioManager 게임 오브젝트가 파괴되지 않음
        }
        else
        {
            Destroy(gameObject);  // 싱글턴이 2개 이상이 될 때 하나를 파괴 처리
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
