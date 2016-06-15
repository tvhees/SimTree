using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PanelDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public RectTransform rTransform;
    public Canvas canvas;

    private Vector2 startPos, endPos;
    private bool released;
    private float startY, t;

    void Update()
    {
        if (released)
            rTransform.anchoredPosition = Vector2.Lerp(endPos, startPos, 10f * (Time.time - t));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startY = eventData.position.y;
        startPos = rTransform.anchoredPosition;
        released = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rTransform.anchoredPosition = new Vector2(0f, Mathf.Min(eventData.position.y - startY, 0)/canvas.scaleFactor);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.position.y <= Screen.height / 6f)
            StartCoroutine(PlayerManager.Instance.Start());
        else
        {
            endPos = rTransform.anchoredPosition;
            t = Time.time;
            released = true;
        }
    }
}
