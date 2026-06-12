using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [Header("Referencias UI")]
    public RectTransform playerIcon;
    public RectTransform mapImage;

    [Header("Jugador")]
    public Transform player;

    [Header("Límites del mundo (X y Z)")]
    public Vector2 worldMin; 
    public Vector2 worldMax;  

    
    void Update()
    {
        ActualizarIcono();
    }

    public void ActualizarIcono()
    {
        Vector2 posJugador = new Vector2(player.position.x, player.position.y);

        float normX = Mathf.InverseLerp(worldMin.x, worldMax.x, posJugador.x);
        float normY = Mathf.InverseLerp(worldMin.y, worldMax.y, posJugador.y);

        float iconX = (normX - 0.5f) * mapImage.sizeDelta.x;
        float iconY = (normY - 0.5f) * mapImage.sizeDelta.y;

        playerIcon.anchoredPosition = new Vector2(iconX, iconY);
    }
}