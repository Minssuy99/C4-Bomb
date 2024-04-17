using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour
{
    public void GoMain()
    {
        if (CompareTag("Stage1"))
        {
            GameManager.sceneVariable.level = 1;
        }
        else if (CompareTag("Stage2"))
        {
            GameManager.sceneVariable.level = 2;
        }
        else if (CompareTag("Stage3"))
        {
            GameManager.sceneVariable.level = 3;
        }

        SceneManager.LoadScene("MainScene");
    }
}