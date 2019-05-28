using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public ModuleConnection moduleConnection;

    public Text inputText;
    public GameObject errorIndicator;

    public void OnStart()
    {
        moduleConnection.PlayerAuthentification(inputText.text, (bool success) => {
            errorIndicator.SetActive(!success);
            if(success)
            {
                SceneManager.LoadScene(1, LoadSceneMode.Single);
            }
        });
    }
}
