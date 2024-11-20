using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private int _startingCoins;

    private int _coinsCount;

    public event Action<int> AmountChanged;

    private void Start()
    {
        _coinsCount = _startingCoins;
        AmountChanged?.Invoke(_coinsCount);
    }

    public bool TrySpendCoins(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Coins amount while spending cant be negative");
        }

        if (amount <= _coinsCount)
        {
            _coinsCount -= amount;
            AmountChanged?.Invoke(_coinsCount);
            return true;
        }

        return false;
    }

    public void AddCoins(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Coins amount while adding cant be negative");
        }

        _coinsCount += amount;
        AmountChanged?.Invoke(_coinsCount);
    }
}
