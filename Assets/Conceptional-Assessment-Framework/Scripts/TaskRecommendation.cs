using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskRecommendation
{
    [SerializeField]
    protected string taskName;      public string TaskName { get { return taskName; } set { taskName = value; } }
    [SerializeField] 
    protected float difficulty;     public float Difficulty { get { return difficulty; } set { difficulty = value; } }
}
