using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    private Dictionary<int, GameObject> itemDictionary;

    private void Awake()
    {
        itemDictionary = new Dictionary<int, GameObject>();

        foreach (Item item in itemPrefabs)
        {
            if (item == null) continue;

            Debug.Log("Registrando: " + item.name + " con ID: " + item.ID); // añade esto


            if (itemDictionary.ContainsKey(item.ID))
            {
                Debug.LogError(" ID DUPLICADO: " + item.ID + " en " + item.name);
                continue;
            }

            itemDictionary.Add(item.ID, item.gameObject);
        }
    }

    public GameObject GetItemPrefab(int itemID)
    {
        if (!itemDictionary.ContainsKey(itemID))
        {
            Debug.LogError(" No existe prefab para ID: " + itemID);
            return null;
        }
        return itemDictionary[itemID];
    }
}