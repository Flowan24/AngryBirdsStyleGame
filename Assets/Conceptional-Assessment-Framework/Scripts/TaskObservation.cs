using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskObservation
{
    [SerializeField]
    protected float[] targetPosition; public float[] TargetPosition { set; get; }

    [SerializeField]
    protected float[] error;        public float[] Error { set; get;}

    [SerializeField]
    protected float difficulty;     public float Difficulty { set;  get; }

    [SerializeField]
    protected float weight;         public float Weight { set;  get; }

    [SerializeField]
    protected string userId;        public string UserId { set;  get; }

    [SerializeField]
    protected string typeId;        public string TypeId { set; get; }
}
