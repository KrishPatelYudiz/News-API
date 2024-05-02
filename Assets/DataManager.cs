using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
    string NEWS_API = "https://newsapi.org/v2/everything?q=tesla&from=2024-04-02&sortBy=publishedAt&apiKey=5a2848f75ccb4920805a8bf669a641ba";
    public static Root data;
    public CardManager card;
    void Start()
    {
      StartCoroutine(GetRequest(NEWS_API));
    }
    IEnumerator GetRequest(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url); 

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success){
            string json = request.downloadHandler.text;
            data = JsonUtility.FromJson<Root>(json);
            card.InitializeCard();
        }else
        {
            Debug.Log("Error: "+request.error);
        }
    }
}
[Serializable]
public class Source
{
    public object id;
    public string name;
}
[Serializable]
public class Article
{
    public Source source;
    public string author;
    public string title;
    public string description;
    public string url;
    public string urlToImage;
    public DateTime publishedAt;
    public string content;
}
[Serializable]

public class Root
{
    public string status;
    public int totalResults;
    public List<Article> articles;
}
