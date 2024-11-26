using UnityEngine;

[CreateAssetMenu(menuName = "SO/Trading/New item", fileName = "New item", order = 42)]
public class ItemSO : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _name;
    [TextArea]
    [SerializeField] private string _descpription;
    [SerializeField] private int _buyPrice;
    [SerializeField] private int _sellPrice;

    public Sprite Sprite => _sprite;
    public int BuyPrice => _buyPrice;
    public int SellPrice => _sellPrice;
    public string Name => _name;
    public string Descpription => _descpription;
}
