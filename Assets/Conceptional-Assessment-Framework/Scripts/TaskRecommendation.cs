using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskRecommendation
{
    [SerializeField]
    protected string name;          public string Name { get; set; }
    [SerializeField] 
    protected float difficulty;     public float Difficulty { get; set; }
}
