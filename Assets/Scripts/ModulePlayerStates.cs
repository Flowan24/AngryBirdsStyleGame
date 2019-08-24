using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePlayerStates : MonoBehaviour
{
    private Vector2 currentTargetPosition;
    private ModuleConnection moduleConnection;

    private TaskRecommendation currentTaskRecommendation;


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        moduleConnection = GameObject.FindObjectOfType<ModuleConnection>();
    }

    public void FetchNextTurn(Action<TaskRecommendation> callback)
    {
        moduleConnection.FetchNextTask((TaskRecommendation taskRecommendation) =>
        {
            Debug.Log("Task Recommendation: " + JsonUtility.ToJson(taskRecommendation));
            currentTaskRecommendation = taskRecommendation;
            callback(taskRecommendation);
        });
    }

    public void TurnEnded(Collision2D collision, GameObject pig)
    {
        TaskObservation taskObservation = new TaskObservation();
        
        taskObservation.TargetPosition = new float[] { currentTargetPosition.x, currentTargetPosition.y };
        taskObservation.Error = new float[2] { 0, 0 };
        taskObservation.TaskName = currentTaskRecommendation.TaskName;
        taskObservation.Difficulty = currentTaskRecommendation.Difficulty;

        //if player failed to hit the pig
        if (pig != null && collision.gameObject != pig)
        {
            //Get distance between bird hit point and pig position
            Vector2 error = collision.GetContact(0).point - (Vector2)pig.transform.position;
            taskObservation.Error[0] = error.x;
            taskObservation.Error[1] = error.y;
        }
        
        moduleConnection.SubmitTaskObservation(taskObservation);
    }

    public string ToJson(bool prettyPrint)
    {
        return JsonUtility.ToJson(this,prettyPrint);
    }

    public Vector3 GenerateTargetPosition()
    {
        currentTargetPosition.x = UnityEngine.Random.Range(0.5f, 6.5f);
        currentTargetPosition.y = -3.5f;

        return (Vector3)currentTargetPosition;
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Print"))
        {
            Debug.Log(ToJson(true));
        }
    }
}
