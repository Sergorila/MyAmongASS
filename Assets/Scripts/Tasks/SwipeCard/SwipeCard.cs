using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeCard : MonoBehaviour, IDragHandler
{
    private Canvas _canvas;

    private Rigidbody2D _rb;

    private bool isDragging = false;

    private Vector2 startPos;

    public GameObject status;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _rb = GetComponent<Rigidbody2D>();
        startPos = _rb.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && status.activeSelf)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                eventData.position,
                _canvas.worldCamera,
                out pos);
            var tr = _canvas.transform.TransformPoint(pos);
            if (tr.x > -7 && tr.x < 7)
            {
                transform.position = new Vector2(tr.x, transform.position.y);
            }
            
        }
        else
        {
            StartCoroutine(MoveCard());
        }
        
    }

    private void OnMouseDown()
    {
        if (status.activeSelf)
        {
            isDragging = true;
        }
    }

    private void OnMouseUp()
    {
        StartCoroutine(MoveCard());
    }

    IEnumerator MoveCard()
    {
        isDragging = false;
        transform.position = Vector2.Lerp(transform.position, startPos, Time.deltaTime * 25);
        yield return new WaitForSeconds(1.5f);
    }
}
