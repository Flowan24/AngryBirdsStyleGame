using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject gameStart;
    public GameObject gameWon;
    public GameObject gameLost;
    public GameObject gameFinished;

    public void OpenGameStart()
    {
        gameStart.SetActive(true);
    }
    public void OpenGameWon()
    {

        gameWon.SetActive(true);
    }

    public void OpenGameLost()
    {

        gameLost.SetActive(true);
    }

    public void CloseMenu()
    {
        gameStart.SetActive(false);
        gameWon.SetActive(false);
        gameLost.SetActive(false);
        gameFinished.SetActive(false);
    }

    public void OpenGameFinished()
    {
        gameFinished.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
