using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Execution : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private NavigationDataHolderSO _navData;
    [SerializeField] private Navigator _navigator;
    [Header("Alert Messages")]
    [SerializeField] private string _wrongEdgesAmountAlert;
    [SerializeField] private string _wrongRectanglesAmountAlert;
    [SerializeField] private string _equalRectanglesAlert;
    [SerializeField] private string _equalEdgesAlert;
    [SerializeField] private string _equalStartFinishAlert;

    private int _minRectangleAmount = 2;

    public event Action<List<Vector2>> ResultsReceived;

    public void Calculate()
    {
        List<Edge> edges = _navData.Edges;
        Vector2 start = _navData.StartPosition;
        Vector2 finish = _navData.FinishPosition;

        if (IsValidInput(out string message) == false)
        {
            AlertSystem.Instance.SendAlert(message);
            return;
        }

        IEnumerable<Vector2> results = _navigator.GetPath(start, finish, edges);
        ResultsReceived?.Invoke(results.ToList());
    }

    private bool IsValidInput(out string message)
    {
        foreach (Rectangle rectangle in _navData.Rectangles)
        {
            if (_navData.Rectangles.Where((testRectangle) => testRectangle.Equals(rectangle)).Count() > 1)
            {
                message = _equalRectanglesAlert;
                return false;
            }
        }

        foreach (Edge edge in _navData.Edges)
        {
            if (_navData.Edges.Where((testEdge) => testEdge.Equals(edge)).Count() > 1)
            {
                message = _equalEdgesAlert;
                return false;
            }
        }

        if (_navData.StartPosition == _navData.FinishPosition)
        {
            message = _equalStartFinishAlert;
            return false;
        }

        if (_navData.Rectangles.Count < _minRectangleAmount)
        {
            message = _wrongRectanglesAmountAlert;
            return false;
        }

        if (_navData.Rectangles.Count - 1 != _navData.Edges.Count)
        {
            message = _wrongEdgesAmountAlert;
            return false;
        }

        message = string.Empty;
        return true;
    }
}
