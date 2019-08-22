using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject button;
    public Text inputText;
    public GameObject errorIndicator;

    private ModuleConnection moduleConnection;

    private void Awake()
    {
        moduleConnection = GameObject.FindObjectOfType<ModuleConnection>();
    }

    public void OnStart()
    {
        button.SetActive(false);

        moduleConnection.PlayerAuthentication(inputText.text, (bool successPlayerAuth) => {
            errorIndicator.SetActive(!successPlayerAuth);
            
            if(successPlayerAuth)
            {
                SceneManager.LoadScene(1, LoadSceneMode.Single);
            }
            else
            {
                button.SetActive(true);
            }

        });


    }
}
