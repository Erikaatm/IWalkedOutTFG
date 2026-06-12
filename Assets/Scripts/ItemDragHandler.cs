using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Guardamos el padre actual (el Slot real en la jerarquía)
        originalParent = transform.parent;

        // Lo movemos a la raíz del Canvas para que se vea por encima de todo
        transform.SetParent(transform.root);

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Slot dropSlot = null;

        // 1. Detectar qué hay bajo el mouse
        GameObject hoveredObj = eventData.pointerEnter;

        if (hoveredObj != null)
        {
            // Intentamos buscar el componente Slot en el objeto tocado
            // o en sus padres (por si tocaste un icono dentro del slot)
            dropSlot = hoveredObj.GetComponentInParent<Slot>();
        }

        // 2. Ejecutar el cambio de padre
        if (dropSlot != null)
        {
            // Lógica de intercambio...
            Slot originalSlot = originalParent.GetComponent<Slot>();

            if (dropSlot.currentItem != null)
            {
                Transform otherItem = dropSlot.currentItem.transform;
                otherItem.SetParent(originalParent);
                otherItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                originalSlot.currentItem = dropSlot.currentItem;
            }
            else
            {
                if (originalSlot != null) originalSlot.currentItem = null;
            }

            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            // SI NO ES UN SLOT (es la página, el fondo, etc.)
            // Forzamos el regreso al Slot original inmediatamente
            transform.SetParent(originalParent);
        }

        // 3. Reajuste visual obligatorio
        ResetRectTransform();
    }

    private void ResetRectTransform()
    {
        // Esto asegura que el ítem vuelva a estar centrado y con el tamaño 100x100
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(100, 100);
    }
}