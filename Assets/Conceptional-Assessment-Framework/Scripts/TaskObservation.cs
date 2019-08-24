using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskObservation
{
    [SerializeField]
    protected float[] targetPosition;       public float[] TargetPosition { get { return targetPosition; } set { targetPosition = value; } }

    [SerializeField]
    protected float[] error;                public float[] Error { get { return error; } set { error = value; } }

    [SerializeField]
    protected float difficulty;             public float Difficulty { get { return difficulty; } set { difficulty = value; } }

    [SerializeField]
    protected float weight;                 public float Weight { get { return weight; } set { weight = value; } }

    [SerializeField]
    protected string user;                public string User { get { return user; } set { user = value; } }


    [SerializeField]
    protected string taskName;              public string TaskName { get { return taskName; } set { taskName = value; } }
}
