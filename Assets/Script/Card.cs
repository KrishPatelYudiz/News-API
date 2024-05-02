using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] RawImage _image;
    [SerializeField] TMP_Text _titleField;
    [SerializeField] TMP_Text _descriptionfield;
    [SerializeField] Button _MoreButton;
    public  Canvas canvas;
    public SwipeEffect swipeEffect;
    

    public int ind = 0;
    public Article article;
    private void Awake() {
        canvas = GetComponent<Canvas>();
        swipeEffect = GetComponent<SwipeEffect>();
        DisableCard();
    }

    public void EnableCard()
    {
        GetComponent<RectTransform>().position = new Vector3(0, 0,0);
        canvas.enabled = true;
        canvas.sortingOrder = 2;
        swipeEffect.enabled=true;
    }
    public void DisableCard()
    {
        canvas.sortingOrder = 0;
        canvas.enabled = false;
        swipeEffect.enabled=false;
    }
    private void Start() {
        _MoreButton.onClick.AddListener(OnMore);
    }
    public void LoadData(){
        article = DataManager.data.articles[ind];
        _titleField.text = article.title;
        _descriptionfield.text = article.description;
        StopAllCoroutines();
        StartCoroutine(GetImage(article.urlToImage));
    }

    void OnMore(){
        Application.OpenURL(article.url);
    }
    IEnumerator GetImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url); 
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success){
            _image.texture = DownloadHandlerTexture.GetContent(request);
        }else
        {
            Debug.Log("Error: "+request.error);
        }
    }
}
