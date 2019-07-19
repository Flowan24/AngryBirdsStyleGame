using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Turn
{
    [SerializeField]
    public int turnNumber;
    [SerializeField]
    public float[] targetPosition;

    public float[] error;


    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

}