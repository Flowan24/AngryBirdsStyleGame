using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject pig;
    private ModulePlayerStates playerStates;

    void Awake()
    {
        playerStates = GameObject.FindObjectOfType<ModulePlayerStates>();
    }

    void Start()
    {
        pig.transform.position = playerStates.GenerateTargetPosition();    
    }
}
