using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour, IDropHandler
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Trade _trade;
    [SerializeField] private CellFactory _cellFactory;
    [SerializeField] private GridLayoutGroup _cellContainer;

    private List<Cell> _cells;

    public event Action<ItemSO> BuyingItem;
    public event Action<ItemSO> SellingItem;

    private void Awake()
    {
        _cells = new List<Cell>();
    }

    private void OnEnable()
    {
        _inventory.AddedItem += AddItem;
        _inventory.RemovedItem += RemoveItem;
    }

    private void OnDisable()
    {
        _inventory.AddedItem -= AddItem;
        _inventory.RemovedItem -= RemoveItem;
    }

    private void AddItem(ItemSO item)
    {
        Cell newCell = _cellFactory.CreateCell(item);
        newCell.transform.SetParent(_cellContainer.transform);
        _cells.Add(newCell);
    }

    private void RemoveItem(ItemSO item)
    {
        Cell cellWithItem = _cells.Where((cell) => cell.CurrentItem == item).First();

        if (cellWithItem != null)
        {
            _cells.Remove(cellWithItem);
            Destroy(cellWithItem.gameObject);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.TryGetComponent(out Cell cell))
        {
            if (_cells.Contains(cell) == false)
            {
                if (_inventory is PlayerInventory)
                {
                    _trade.BuyItemFromTrader(cell.CurrentItem);
                }
                else
                {
                    _trade.SellPlayerItem(cell.CurrentItem);
                }
            }
        }
    }
}
