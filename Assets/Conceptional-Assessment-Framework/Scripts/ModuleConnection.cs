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
    public string UserId
    {
        get { return userId; }
    }
    private string gameId = "";
    public string GameId
    {
        get { return gameId; }
    }
    private List<TaskRecommendation> exerciseTypes;
    public 

    GraphQLClient client;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
        client = new GraphQLClient(apiUrl);
    }

    #region Player Authentication
    public void PlayerAuthentication(string accessToken, Action<bool> callback=null)
    {
        StartCoroutine(AuthentifyingPlayer(accessToken, callback));
    }

    private IEnumerator AuthentifyingPlayer(string accessToken, Action<bool> callback)
    {
        string query = @"query authUser($accessToken:String!) { authUser(accessToken:$accessToken) {_id}}";

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
                bool isError = responseString.Contains("error");
                if (!isError)
                {
                    JSONObject response = new JSONObject(responseString);
                    userId = response.GetField("data").GetField("authUser").GetField("_id").str;
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
        string query = "query fetchNextTaskType {fetchNextTask(userId:\"" + this.userId+ "\" ){name,difficulty}}";

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
                
                bool isError = responseString.Contains("error");

                TaskRecommendation nextTask = null;
                if (!isError)
                {
                    nextTask = new TaskRecommendation();
                    JSONObject response = new JSONObject(responseString);
                    nextTask.Name = response.GetField("data").GetField("fetchNextTask").GetField("name").str;
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
        StartCoroutine(UploadingTaskObservation(taskObservation, callback));
    }

    private IEnumerator UploadingTaskObservation(TaskObservation taskObservation, Action<bool> callback)
    {
        string query = @"mutation submitTaskObservations ($in: TaskObservationsInput!) {submitTaskObservations(taskObservations: $in) {_id}}";
        
        string variable = "{ \"in\": " + JsonUtility.ToJson(taskObservation) + "}";

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

                bool isError = responseString.Contains("error");
                
                if (isError)
                {
                    Debug.Log("error:" + responseString);
                }
                
                callback?.Invoke(!isError);
            }
        }
    }
#endregion

    public void accessData(JSONObject obj)
    {
        switch (obj.type)
        {
            case JSONObject.Type.OBJECT:
                for (int i = 0; i < obj.list.Count; i++)
                {
                    string key = (string)obj.keys[i];
                    JSONObject j = (JSONObject)obj.list[i];
                    Debug.Log(key);
                    this.accessData(j);
                }
                break;
            case JSONObject.Type.ARRAY:
                foreach (JSONObject j in obj.list)
                {
                    this.accessData(j);
                }
                break;
            case JSONObject.Type.STRING:
                Debug.Log(obj.str);
                break;
            case JSONObject.Type.NUMBER:
                Debug.Log(obj.n);
                break;
            case JSONObject.Type.BOOL:
                Debug.Log(obj.b);
                break;
            case JSONObject.Type.NULL:
                Debug.Log("NULL");
                break;

        }
    }
}
