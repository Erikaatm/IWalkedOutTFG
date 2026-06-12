using UnityEngine;
public class DynamicSorting : MonoBehaviour
{
    public Transform player;
    public float yOffset = 0f; // ajusta manualmente si el pivot está raro
    SpriteRenderer sr;
    SpriteRenderer[] childRenderers;
    float lastPlayerY;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        childRenderers = GetComponentsInChildren<SpriteRenderer>();
        lastPlayerY = player.position.y;
        UpdateLayer();
    }

    void LateUpdate()
    {
        if (player.position.y == lastPlayerY) return;
        lastPlayerY = player.position.y;
        UpdateLayer();
    }

    void UpdateLayer()
    {
        SpriteRenderer playerSR = player.GetComponentInChildren<SpriteRenderer>();
        float playerFeet = player.position.y - playerSR.bounds.extents.y;

        // Pies del objeto usando su propio SpriteRenderer
        float objectFeet = transform.position.y - sr.bounds.extents.y + yOffset;

        string parentLayer = playerFeet < objectFeet ? "WalkInFront" : "WalkBehind";
        sr.sortingLayerName = parentLayer;

        foreach (SpriteRenderer childSR in childRenderers)
        {
            if (childSR == sr) continue;
            childSR.sortingLayerName = parentLayer;
        }
    }
}