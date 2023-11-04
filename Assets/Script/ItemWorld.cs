using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAsset.Instance._pfItemWorld, position, Quaternion.identity);
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);
        return itemWorld;
    }
    Item _item;
    public void SetItem(Item item)
    {
        this._item = item;
    }
}
