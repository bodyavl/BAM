using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public void ChangeScenes(int numberScenes)
    {
        SceneManager.LoadScene(numberScenes);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToNextLevelOrRestart()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        UnityEngine.Debug.Log(currentScene.buildIndex);

        if (currentScene.buildIndex < SceneManager.sceneCountInBuildSettings) SceneManager.LoadScene(currentScene.buildIndex + 1);
        else SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
