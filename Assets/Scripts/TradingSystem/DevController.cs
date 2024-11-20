using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevController : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private TraderInventory _traderInventory;
    [SerializeField] private TMP_InputField _playerItemIndex;
    [SerializeField] private TMP_InputField _traderItemIndex;
    [SerializeField] private List<ItemSO> _items;

    public void AddRandomItemForPlayer()
    {
        _playerInventory.AddItem(_items[Random.Range(0, _items.Count)]);
    }

    public void AddRandomItemForTrader()
    {
        _traderInventory.AddItem(_items[Random.Range(0, _items.Count)]);
    }

    public void RemoveItemFromPlayer()
    {
        if (int.TryParse(_playerItemIndex.text, out int index))
        {
            _playerInventory.RemoveItem(index);
        }
    }

    public void RemoveItemFromTrader()
    {
        if (int.TryParse(_traderItemIndex.text, out int index))
        {
            _traderInventory.RemoveItem(index);
        }
    }
}
