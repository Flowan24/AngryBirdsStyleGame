using GraphQL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ModuleConnection : MonoBehaviour
{
    public string apiUrl = "http://localhost:5000/graphql";
    private string userId = "";
    private GraphQLClient client;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        client = new GraphQLClient(apiUrl);
    }

    public int Turns { get; private set; } = 0;
    public int Type { get; private set; } = 0;

    #region Player Authentication
    public void PlayerAuthentication(string accessToken, Action<bool> callback=null)
    {
        StartCoroutine(AuthentifyingPlayer(accessToken, callback));
    }

    private IEnumerator AuthentifyingPlayer(string accessToken, Action<bool> callback)
    {
        string query = @"query authUser($accessToken:String!) { authUser(accessToken:$accessToken) {_id, turns,type}}";

        string variable = "{\"accessToken\":\""+ accessToken + "\"}";

        using (UnityWebRequest www = client.Query(query, variable, "authUser"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);

                callback?.Invoke(false);
            }
            else
            {
                string responseString = www.downloadHandler.text;
                bool isError = IsResponseError(responseString);
                Debug.Log(responseString);
                if (!isError)
                {
                    JSONObject response = new JSONObject(responseString);
                    userId = response.GetField("data").GetField("authUser").GetField("_id").str;
                    Turns = Convert.ToInt16(response.GetField("data").GetField("authUser").GetField("turns").i);
                    Type = Convert.ToInt16(response.GetField("data").GetField("authUser").GetField("type").i);
                }
                else
                {
                    Debug.Log("error:" + responseString);
                }

                callback?.Invoke(!isError);
            }
        }
    }
    #endregion

    #region Fetch Next Exercise

    internal void FetchNextTask(Action<TaskRecommendation> callback)
    {
        StartCoroutine(fetchingNextTask(callback));
    }

    private IEnumerator fetchingNextTask(Action<TaskRecommendation> callback)
    {
        string query = "query fetchNextTaskType {fetchNextTask(userId:\"" + this.userId+ "\" ){taskName,difficulty}}";

        string variable = "{}";

        using (UnityWebRequest www = client.Query(query, variable, "fetchNextTaskType"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);

                callback?.Invoke(null);
            }
            else
            {
                string responseString = www.downloadHandler.text;
                
                bool isError = IsResponseError(responseString);

                TaskRecommendation nextTask = null;
                if (!isError)
                {
                    nextTask = new TaskRecommendation();
                    JSONObject response = new JSONObject(responseString);
                    nextTask.TaskName = response.GetField("data").GetField("fetchNextTask").GetField("taskName").str;
                    nextTask.Difficulty = response.GetField("data").GetField("fetchNextTask").GetField("difficulty").n;
                }
                else
                {
                    Debug.Log("error:" + responseString);
                }

                callback?.Invoke(nextTask);
            }
        }
    }

    #endregion

    #region Submit TaskObservation
    public void SubmitTaskObservation(TaskObservation taskObservation, Action<bool> callback = null)
    {
        taskObservation.User = userId;
        StartCoroutine(UploadingTaskObservation(taskObservation, callback));
    }

    private IEnumerator UploadingTaskObservation(TaskObservation taskObservation, Action<bool> callback)
    {
        string query = @"mutation submitTaskObservations ($in: TaskObservationsInput!) {submitTaskObservations(taskObservations: $in) {_id}}";

        string variable = "{ \"in\": " + JsonUtility.ToJson(taskObservation) + "}";

        Debug.Log("Task Observation: " + JsonUtility.ToJson(taskObservation));

        using (UnityWebRequest www = client.Query(query, variable, "submitTaskObservations"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);

                callback?.Invoke(false);
            }
            else
            {
                string responseString = www.downloadHandler.text;

                bool isError = IsResponseError(responseString);

                if (isError) {
                    Debug.Log("response:" + responseString);
                }
                else {
                    this.Turns++;
                }
                callback?.Invoke(!isError);
            }
        }
    }
#endregion

    private bool IsResponseError(string response)
    {
        return response.Contains("error") || response.Contains("invalid");
    }
}
