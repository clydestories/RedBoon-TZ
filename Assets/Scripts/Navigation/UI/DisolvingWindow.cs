using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class DisolvingWindow : MonoBehaviour
{
    protected CanvasGroup CanvasGroup;
    protected Coroutine DisolvingCoroutine;
    protected float AlertOnAlpha = 1;

    [Header("Settings")]
    [SerializeField] private float _alertDisolveDelay;
    [SerializeField] private float _alertDisolveTime;
    [SerializeField] private float _disolveStep;

    private float _alertOffAlpha = 0;
    private float _alertAlphaToDisappear = 0.01f;

    private void Awake()
    {
        Initialize();
    }

    protected void Initialize()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    protected IEnumerator Disolving()
    {
        yield return new WaitForSeconds(_alertDisolveDelay);
        var wait = new WaitForSeconds(_alertDisolveTime * _disolveStep);

        for (float i = CanvasGroup.alpha; CanvasGroup.alpha > _alertAlphaToDisappear; i += _disolveStep)
        {
            CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, _alertOffAlpha, _disolveStep);
            yield return wait;
        }

        CanvasGroup.alpha = _alertOffAlpha;

        DisolvingCoroutine = null;
    }
}
