using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public ItemCategoria categoria;

    public virtual void UseItem()
    {
        Debug.Log("Using item " + ID);
    }
}
