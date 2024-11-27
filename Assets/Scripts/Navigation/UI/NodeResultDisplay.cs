using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeResultDisplay : DisolvingWindow
{
    [Header("Dependencies")]
    [SerializeField] private LayoutGroup _textContainer;
    [SerializeField] private TextMeshProUGUI _textPrefab;
    [SerializeField] private Execution _exec;

    private void OnEnable()
    {
        _exec.ResultsReceived += DisplayNodes;
    }

    private void OnDisable()
    {
        _exec.ResultsReceived -= DisplayNodes;
    }

    private void DisplayNodes(List<Vector2> nodes)
    {
        foreach (var text in _textContainer.GetComponentsInChildren<TextMeshProUGUI>())
        {
            Destroy(text.gameObject);
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            TextMeshProUGUI newText = Instantiate(_textPrefab, _textContainer.transform);
            newText.text = $"Node {i} - {nodes[i]}";
        }

        CanvasGroup.alpha = AlertOnAlpha;

        if (DisolvingCoroutine != null)
        {
            StopCoroutine(DisolvingCoroutine);
        }

        DisolvingCoroutine = StartCoroutine(Disolving());
    }
}
