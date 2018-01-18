using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour {

    public void loadSelectedScene(int value)
    {
        switch (value)
        {
            case 0:
                //SceneManager.
                break;
            case 1:
                SceneManager.LoadScene("MenuScene");
                //SceneManager.UnloadSceneAsync("GameScene");

                break;
        }
    }
}
