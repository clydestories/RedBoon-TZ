using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Execution : MonoBehaviour
{
    [SerializeField] private Visualizer _visualizer;
    [SerializeField] private float _step;

    [SerializeField] private Navigator _navigator;

    private void Start()
    {
        Test();
    }

    public void Test()
    {
        List<Rectangle> rectangles = new List<Rectangle>
        {
            new Rectangle(new Vector2(-15, 15), new Vector2(2, 25)),
            new Rectangle(new Vector2(-3, 25), new Vector2(17, 35)),
            new Rectangle(new Vector2(17, 20), new Vector2(37, 30)),
            new Rectangle(new Vector2(37, 20), new Vector2(42, 30)),
            new Rectangle(new Vector2(40, 30), new Vector2(50, 35)),
        };

        List<Edge> edges = new List<Edge>
        {
            new Edge(
                rectangles[0],
                rectangles[1],
                new Vector2(-3, 25),
                new Vector2(2, 25)
                ),

            new Edge(
                rectangles[1],
                rectangles[2],
                new Vector2(17, 25),
                new Vector2(17, 30)
                ),

            new Edge(
                rectangles[2],
                rectangles[3],
                new Vector2(37, 20),
                new Vector2(37, 30)
                ),

            new Edge(
                rectangles[3],
                rectangles[4],
                new Vector2(40, 30),
                new Vector2(42, 30)
                )
        };

        Vector2 start = new Vector2(-6.5f, 15);
        Vector2 finish = new Vector2(40, 32);

        foreach (Rectangle rectangle in rectangles)
        {
            _visualizer.DrawRectangle(rectangle, Color.red);
        }

        foreach (Edge edge in edges)
        {
            _visualizer.DrawLine(edge.Start, edge.End, Color.green);
        }

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
