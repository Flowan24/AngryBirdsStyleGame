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
    private string username = "";
    GraphQLClient client;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        username = PlayerPrefs.GetString(Constants.KeyPlayerPrefsUser);

        client = new GraphQLClient(apiUrl);

        if(string.IsNullOrEmpty(username))
        {
            StartCoroutine(PlayerRegistration(OnPlayerAuthentication));
        }
    }

    private void OnPlayerAuthentication(bool success)
    {
        Debug.Log("Query success: "+ success);
    }

    public IEnumerator PlayerRegistration(Action<bool> callback)
    {
        string query = @"query getUser {" +
                       @"user(_id:""5cebd34e71892b47a8f8997b"")" +
                       @"{ _id,email,games{ _id, totalTurns} }" +
                       "}";

        string variable = "{}";

        using (UnityWebRequest www = client.Query(query, variable, "getUser"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);

                callback(false);
            }
            else
            {
                string responseString = www.downloadHandler.text;
                //JSONObject response = new JSONObject(responseString);
                //JSONObject data = response.GetField("data");
                //JSONObject user = data.GetField("user");
                //accessData(user);
                Debug.Log(responseString);

                callback(true);
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
