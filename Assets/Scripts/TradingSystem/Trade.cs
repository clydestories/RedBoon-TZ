using UnityEngine;

public class Trade : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private Wallet _playerWallet;
    [Header("Trader")]
    [SerializeField] private TraderInventory _traderInventory;
    [SerializeField] private Wallet _traderWallet;

    public void SellPlayerItem(ItemSO item)
    {
        if (_traderWallet.TrySpendCoins(item.SellPrice))
        {
            _traderInventory.AddItem(item);
            _playerInventory.RemoveItem(item);
            _playerWallet.AddCoins(item.SellPrice);
        }
    }

    public void BuyItemFromTrader(ItemSO item)
    {
        if (_playerWallet.TrySpendCoins(item.BuyPrice))
        {
            _traderWallet.AddCoins(item.BuyPrice);
            _traderInventory.RemoveItem(item);
            _playerInventory.AddItem(item);
        }
    }
}
