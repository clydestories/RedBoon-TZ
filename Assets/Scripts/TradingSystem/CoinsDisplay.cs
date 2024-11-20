using TMPro;
using UnityEngine;

public class CoinsDisplay : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TextMeshProUGUI _textContainer;

    private void OnEnable()
    {
        _wallet.AmountChanged += UpdateValue;
    }

    private void OnDisable()
    {
        _wallet.AmountChanged -= UpdateValue;
    }

    private void UpdateValue(int amount)
    {
        _textContainer.text = amount.ToString();
    }
}
