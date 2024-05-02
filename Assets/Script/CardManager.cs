using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform CardParent;
    private int numberOfCards= 25;
    private int halfNumbers => numberOfCards/2;
    private int currIndex = 5;
    private Card currentCard;
    private List<Card> cardList = new List<Card>();
      public delegate void OnNextPage();
    public static event OnNextPage onNextPage;
      public delegate void OnPreviousPage();
    public static event OnPreviousPage onPreviousPage;
    private void Awake() {
        onNextPage += OnNext;
        onPreviousPage += OnPrevious;
    }
    public static void TriggerNextPage()
    {
        onNextPage?.Invoke();
    }
    public static void TriggerPreviousPage()
    {
        onPreviousPage?.Invoke();
    }
    public void InitializeCard() {
        for (int i = 0;i<numberOfCards;i++)
        {
            GameObject cardObj =Instantiate(cardPrefab,CardParent);
            var card = cardObj.GetComponent<Card>();
            card.ind = i;
            cardList.Add(card);
            card.LoadData();
            card.DisableCard();
        }
        currentCard = cardList[currIndex];
        UpdateCurrentCard();

    }
     void OnNext()
    {
        foreach (var card in cardList)
        {
            if( card.ind == currIndex + halfNumbers+1){
                currentCard.DisableCard();
                card.LoadData();
                currIndex++;
                UpdateCurrentCard();
            }
        }
        foreach (var card in cardList)
        {
            if (currIndex == card.ind + halfNumbers)
            {
                card.ind = currIndex + halfNumbers+1;
                currentCard.DisableCard();


                card.LoadData();
                currIndex++;
                UpdateCurrentCard();
                return;

            }
        }
    }
     void OnPrevious()
    {
        if ( currIndex == 0)
        {
            return;
        }
        if (currIndex - halfNumbers <= 0 )
        {
            currentCard.DisableCard();
            currIndex--;
            UpdateCurrentCard();
            return;
        }
        foreach (var card in cardList)
        {
            if (currIndex == card.ind - halfNumbers)
            {
                card.ind = currIndex - halfNumbers-1;
                card.LoadData();

                currentCard.DisableCard();

                currIndex--;
                UpdateCurrentCard();
                return;

            }
        }

    }
    private void UpdateCurrentCard(){
        foreach (var card in cardList)
        {
            if (currIndex == card.ind )
            {
                currentCard = card; 
                currentCard.EnableCard();

                SetPreviousCard();
                TurnOnNextCard();
            break;
            }
        }
    }

    void SetPreviousCard(){
       foreach (var card in cardList)
        {
            if (currIndex - 1 == card.ind )
            {
                currentCard.swipeEffect.previousCard = card.swipeEffect;
                var rectTransform =card.swipeEffect.GetComponent<RectTransform>();
                card.canvas.enabled = true;
                card.canvas.sortingOrder = 4;

                rectTransform.localPosition = new Vector3(0,Camera.main.pixelHeight,0); 
                return;
            }
        }
    }
    void TurnOnNextCard()
    {
        foreach (var card in cardList)
        {
            if (currIndex + 1 == card.ind )
            {
                card.canvas.sortingOrder = 1;

                card.canvas.enabled = true;
                break;
            }
        }
    }
}
