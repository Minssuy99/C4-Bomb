using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    public GameObject Stage2Btn;
    public GameObject Stage3Btn;

    void Start()
    {
        Time.timeScale = 1.0f;

        AudioManager am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        am.audioSource.pitch = 1.0f;

        Image Stage2BtnImage = Stage2Btn.transform.GetComponent<Image>();
        Image Stage3BtnImage = Stage3Btn.transform.GetComponent<Image>();

        Color grey = new Color(0.2f, 0.2f, 0.2f);

        switch (GameManager.sceneVariable.openStage)
        {
            case 1:
                Stage2BtnImage.color = grey;
                Stage3BtnImage.color = grey;
                break;
            case 2:
                Stage3BtnImage.color = grey;
                break;
            case 3:
                break;
            default:
                Stage2BtnImage.color = grey;
                Stage3BtnImage.color = grey;
                GameManager.sceneVariable.openStage = 1;
                break;
        }
    }
}
