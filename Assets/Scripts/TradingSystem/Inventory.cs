using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    private List<ItemSO> _items;

    public event Action<ItemSO> AddedItem;
    public event Action<ItemSO> RemovedItem;

    private void Awake()
    {
        _items = new List<ItemSO>();
    }

    public void AddItem(ItemSO item)
    {
        _items.Add(item);
        AddedItem?.Invoke(item);
    }

    public void RemoveItem(ItemSO item)
    {
        _items.Remove(item);
        RemovedItem?.Invoke(item);
    }

    public void RemoveItem(int index)
    {
        if (index < _items.Count && index >= 0)
        {
            ItemSO item = _items[index];
            _items.Remove(item);
            RemovedItem?.Invoke(item);
        }
    }
}
