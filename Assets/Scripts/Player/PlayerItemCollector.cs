using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;
    private SaveController saveController;
    private Item itemInRange;
    private GameObject itemGameObject;
    private Vector3 originalScale;
    public GameObject pickupText;

    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
        saveController = FindObjectOfType<SaveController>();
        pickupText.SetActive(false);
    }

    void Update()
    {
        if (itemInRange != null && Input.GetKeyDown(KeyCode.E))
        {
            bool itemAdded = inventoryController.AddItem(itemGameObject);
            if (itemAdded)
            {
                ItemWorld worldItem = itemGameObject.GetComponent<ItemWorld>();
                if (worldItem != null)
                    saveController.MarcarObjetoCogido(worldItem.worldID);

                RemoveHighlight();
                pickupText.SetActive(false);
                Destroy(itemGameObject);
                itemInRange = null;
                itemGameObject = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            itemInRange = collision.GetComponent<Item>();
            if (itemInRange != null)
            {
                itemGameObject = collision.gameObject;
                originalScale = itemGameObject.transform.localScale;
                HighlightItem();
                pickupText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            RemoveHighlight();
            pickupText.SetActive(false);
            itemInRange = null;
            itemGameObject = null;
        }
    }

    void HighlightItem()
    {
        if (itemGameObject != null)
            itemGameObject.transform.localScale = originalScale * 1.15f;
    }

    void RemoveHighlight()
    {
        if (itemGameObject != null)
            itemGameObject.transform.localScale = originalScale;
    }
}