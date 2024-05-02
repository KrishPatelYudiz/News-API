using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeEffect : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    private Vector3 _initialPosition;
    private Vector3 _previousCardinitialPosition;
    private float _distanceMoved;
    private float _previousCardDistanceMoved;
    private bool _swipeLeft;

    public SwipeEffect previousCard;


    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.delta.y > 0){
            if(previousCard.transform.localPosition.y < _previousCardinitialPosition.y){

                previousCard.transform.localPosition = new Vector2(previousCard.transform.localPosition.x,previousCard.transform.localPosition.y+eventData.delta.y);
            }else{
                transform.localPosition = new Vector2(transform.localPosition.x,transform.localPosition.y+eventData.delta.y);
            }
        }else
        {
             if(transform.localPosition.y > _initialPosition.y){

                transform.localPosition = new Vector2(transform.localPosition.x,transform.localPosition.y+eventData.delta.y);
            }
            else{
                previousCard.transform.localPosition = new Vector2(previousCard.transform.localPosition.x,previousCard.transform.localPosition.y+eventData.delta.y);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _initialPosition = transform.localPosition;
        _previousCardinitialPosition = previousCard.transform.localPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _distanceMoved = Mathf.Abs(transform.localPosition.y - _initialPosition.y);
        _previousCardDistanceMoved = Mathf.Abs(previousCard.transform.localPosition.y - _previousCardinitialPosition.y);
        
        if(_distanceMoved < 0.2 * Screen.height)
        {
            transform.localPosition = _initialPosition;
        }else
        {
            StartCoroutine(MovedCard());
            
        }


        if(_previousCardDistanceMoved < 0.2 * Screen.height)
        {
            previousCard.transform.localPosition = _previousCardinitialPosition;
        }
        else
        {
            StartCoroutine(MovedPreviousCard());
        }
    }

    private IEnumerator MovedCard()
    {
        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime;
            
            
            transform.localPosition = new Vector3(transform.localPosition.x,Mathf.SmoothStep(transform.localPosition.y,
            transform.localPosition.y+Screen.width,time),0);
            
            
            yield return null;
        }
        CardManager.TriggerNextPage();
    }
    private IEnumerator MovedPreviousCard()
    {
        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime;
            
            
            previousCard.transform.localPosition = new Vector3(previousCard.transform.localPosition.x,
            Mathf.SmoothStep(previousCard.transform.localPosition.y,
            
                            _initialPosition.y,time)
            
            ,0);
            
            
            yield return null;
        }
        CardManager.TriggerPreviousPage();
    }
}
