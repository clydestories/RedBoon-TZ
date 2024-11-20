using UnityEngine;

public class CellFactory : MonoBehaviour
{
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Canvas _canvas;

    public Cell CreateCell(ItemSO item)
    {
        Cell newCell = Instantiate(_cellPrefab);
        newCell.Construct(item, _canvas);
        return newCell;
    }
}
