using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IngredienteArrastrable : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int orden;
    public Vector3 posicionOriginal;
    private CanvasGroup canvasGroup;
    private Transform padreOriginal;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        padreOriginal = transform.parent;
        posicionOriginal = transform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling(); 
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        transform.localPosition = posicionOriginal;
    }
}