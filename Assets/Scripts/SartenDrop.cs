using UnityEngine;
using UnityEngine.EventSystems;

public class SartenDrop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        IngredienteArrastrable ingrediente = 
            eventData.pointerDrag.GetComponent<IngredienteArrastrable>();

        if( ingrediente != null )
        {
            MinijuegoCocina.Instance.AnadirIngrediente(ingrediente);
        }
    }
}
