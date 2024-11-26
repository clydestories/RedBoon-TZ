using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Execution : MonoBehaviour
{
    [SerializeField] private Visualizer _visualizer;
    [SerializeField] private NavigationDataHolderSO _navData;
    [SerializeField] private float _step;

    [SerializeField] private Navigator _navigator;

    public void Calculate()
    {
        List<Rectangle> rectangles = _navData.Rectangles;
        List<Edge> edges = _navData.Edges;
        Vector2 start = _navData.StartPosition;
        Vector2 finish = _navData.FinishPosition;

        IEnumerable<Vector2> result = _navigator.GetPath(start, finish, edges);
        List<Vector2> resultAsList = result.ToList();

        for (int i = 0; i < resultAsList.Count - 1; i++)
        {
            _visualizer.DrawLine(resultAsList[i], resultAsList[i + 1], Color.blue);
        }

        _visualizer.DrawPoint(start, Color.yellow);
        _visualizer.DrawPoint(finish, Color.yellow);

        foreach (Vector2 resulty in resultAsList)
        {
            Debug.Log(resulty);
        }
    }
}
