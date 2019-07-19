using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePlayerStates : MonoBehaviour
{
    [SerializeField]
    private int totalTurns = 0;
    [SerializeField]
    private List<Turn> turns;

    private Vector2 currentTargetPosition;
    private ModuleConnection moduleConnection;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        moduleConnection = GameObject.FindObjectOfType<ModuleConnection>();

        totalTurns = 0;
        turns = new List<Turn>();
    }

    public void TurnEnded(Collision2D collision, GameObject pig)
    {
        totalTurns++;

        Turn turn = new Turn();
        turn.turnNumber = totalTurns;
        turn.targetPosition = new float[] { currentTargetPosition.x, currentTargetPosition.y };
        turn.error = new float[2] { 0, 0 };

        //if player failed to hit the pig
        if (pig != null && collision.gameObject != pig)
        {
            //Get distance between bird hit point and pig position
            Vector2 error = collision.GetContact(0).point - (Vector2)pig.transform.position;
            turn.error[0] = error.x;
            turn.error[1] = error.y;
        }
        
        turns.Add(turn);


        moduleConnection.TurnUpload(moduleConnection.GameId, turn);
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
