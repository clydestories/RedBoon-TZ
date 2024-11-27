using TMPro;
using UnityEngine;

public class AlertSystem : DisolvingWindow
{
    [Header("Dependencies")]
    [SerializeField] private TextMeshProUGUI _contentText;

    public static AlertSystem Instance { get; private set; }

    private void Awake()
    {
        Initialize();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SendAlert(string text)
    {
        _contentText.text = text;
        CanvasGroup.alpha = AlertOnAlpha;

        if (DisolvingCoroutine != null)
        {
            StopCoroutine(DisolvingCoroutine);
        }

        DisolvingCoroutine = StartCoroutine(Disolving());
    }
}
