using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject gameStart;
    public GameObject gameWon;
    public GameObject gameLost;

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
    }
}
