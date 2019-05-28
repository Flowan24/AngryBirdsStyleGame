using GraphQL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Assets.Scripts;

public class ModuleConnection : MonoBehaviour
{
    public string apiUrl = "http://localhost:4000/graphql";
    public string userId = "";
    GraphQLClient client;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
        client = new GraphQLClient(apiUrl);
    }

    public void PlayerAuthentification(string accessToken, Action<bool> callback=null)
    {
        StartCoroutine(AuthentifyingPlayer(accessToken, callback));
    }

    private IEnumerator AuthentifyingPlayer(string accessToken, Action<bool> callback)
    {
        string query = @"query getUser($userAccesstoken:String!) { user(accessToken:$userAccesstoken) {_id}}";

        string variable = "{\"userAccesstoken\":\""+ accessToken + "\"}";

        using (UnityWebRequest www = client.Query(query, variable, "getUser"))
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
                    userId = response.GetField("data").GetField("user").GetField("_id").str;
                }

                callback?.Invoke(!isError);
            }
        }
    }

    private void accessData(JSONObject obj)
    {
        switch (obj.type)
        {
            case JSONObject.Type.OBJECT:
                for (int i = 0; i < obj.list.Count; i++)
                {
                    string key = (string)obj.keys[i];
                    JSONObject j = (JSONObject)obj.list[i];
                    Debug.Log(key);
                    accessData(j);
                }
                break;
            case JSONObject.Type.ARRAY:
                foreach (JSONObject j in obj.list)
                {
                    accessData(j);
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
