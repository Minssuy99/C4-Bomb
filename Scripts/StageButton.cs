using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour
{
    public Text lockedTxt;
    public GameObject lockedPanel;

    public void GoMain()
    {
        if (CompareTag("Stage1"))
        {
            GameManager.sceneVariable.level = 1;
            SceneManager.LoadScene("MainScene");
        }
        else if (CompareTag("Stage2"))
        {
            if (GameManager.sceneVariable.openStage < 2)
            {
                lockedTxt.text = "Stage 1 클리어 후\n해금됩니다!";
                lockedPanel.SetActive(true);
                Invoke("DestroyLockedPanel", 1.5f);
            }
            else
            {
                GameManager.sceneVariable.level = 2;
                SceneManager.LoadScene("MainScene");
            }
        }
        else if (CompareTag("Stage3"))
        {
            if (GameManager.sceneVariable.openStage < 3)
            {
                lockedTxt.text = "Stage 2 클리어 후\n해금됩니다!";
                lockedPanel.SetActive(true);
                Invoke("DestroyLockedPanel", 1.5f);
            }
            else
            {
                GameManager.sceneVariable.level = 3;
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    void DestroyLockedPanel()
    {
        lockedPanel.SetActive(false);
    }
}
