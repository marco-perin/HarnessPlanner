using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]

public class ConnectionPrefabManager : MonoBehaviourGraphicInstanced
{
    public Transform From;
    public Transform To;
    public EdgeCollider2D EdgeCollider;
    public LineRenderer LineRenderer;

    public int pointNumber = 100;
    public static IEnumerable<float> points;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(From != null);
        Debug.Assert(To != null);

        if (EdgeCollider == null)
            EdgeCollider = GetComponent<EdgeCollider2D>();

        if (LineRenderer == null)
            LineRenderer = GetComponent<LineRenderer>();

        points = Enumerable.Range(0, pointNumber + 1).Select(n => (float)n / (float)pointNumber);
        LineRenderer.positionCount = pointNumber + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (EdgeCollider == null) return;
        if (LineRenderer == null) return;

        //var points = EdgeCollider.points;
        ////points[0] = new Vector3(From.position.x, From.position.z);
        //points[0] = From.position;
        ////points[1] = new Vector3(To.position.x, To.position.z);
        //points[1] = To.position;
        //EdgeCollider.SetPoints(points.ToList());

#if UNITY_EDITOR 
#if DEBUG

        if (Application.isEditor && !Application.isPlaying)
        {
            points = Enumerable.Range(0, pointNumber + 1).Select(n => (float)n / (float)pointNumber);
            LineRenderer.positionCount = pointNumber + 1;
        }
#endif
#endif
        LineRenderer.SetPositions(points.Select(t => Vector3.Lerp(From.position, To.position, t)).ToArray());
        EdgeCollider.SetPoints(points.Select(t => Vector2.Lerp(From.position, To.position, t)).ToList());
    }

}

[CustomEditor(typeof(ConnectionPrefabManager))]
public class ConnectionPrefabManagerEditor : Editor
{
    public void OnSceneGUI()
    {
        var t = target as ConnectionPrefabManager;
        if (t == null)
            return;

        t.From.position = Handles.DoPositionHandle(t.From.position, Quaternion.identity);
        t.To.position = Handles.DoPositionHandle(t.To.position, Quaternion.identity);
    }
}