using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject pig;

    public void Start()
    {
        GameObject goPlayerStates = GameObject.FindGameObjectWithTag("PlayerStates");
        pig.transform.position = goPlayerStates.GetComponent<PlayerStates>().GenerateTargetPosition();
    }
}
