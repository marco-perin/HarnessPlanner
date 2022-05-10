using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class ConnectionPrefabManager : MonoBehaviourGraphicInstanced, IPointerClickHandler
{
    public Transform From;
    public Transform To;
    public Canvas LengthTextCanvas;
    public TMP_Text LengthText;
    public TMP_Text DataText;
    public EdgeCollider2D EdgeCollider;
    public LineRenderer LineRenderer;

    [Range(2, 200)]
    public int pointNumber = 100;

    public float textDistance = 0.3f;
    public float infoDistance = 0.4f;

    public static IEnumerable<float> points;

    void Start()
    {
        Debug.Assert(From != null);
        Debug.Assert(To != null);
        Debug.Assert(LengthTextCanvas != null);
        Debug.Assert(LengthText != null);
        Debug.Assert(DataText != null);

        if (EdgeCollider == null)
            EdgeCollider = GetComponent<EdgeCollider2D>();

        if (LineRenderer == null)
            LineRenderer = GetComponent<LineRenderer>();

        Debug.Assert(pointNumber >= 2);
        points = Enumerable.Range(0, pointNumber + 1).Select(n => (float)n / (float)pointNumber);
        LineRenderer.positionCount = pointNumber + 1;
        UpdatePoints();
    }

    void Update()
    {
        if (EdgeCollider == null) return;
        if (LineRenderer == null) return;
        if (LengthTextCanvas == null) return;
        if (LengthText == null) return;
        if (DataText == null) return;

#if UNITY_EDITOR
#if DEBUG

        if (Application.isEditor && !Application.isPlaying)
        {
            points = Enumerable.Range(0, pointNumber + 1).Select(n => (float)n / (float)pointNumber);
            LineRenderer.positionCount = pointNumber + 1;
        }

#endif
#endif


        var midpoint = Vector3.Lerp(From.position, To.position, 0.5f);

        LengthTextCanvas.transform.position = midpoint;

        var rot = Quaternion.FromToRotation(Vector3.right, To.position - From.position);
        //var angle = rot.eulerAngles.z;
        var updir = rot * Vector3.up;

        if (Vector3.Dot(updir, Vector3.up) < 0)
            updir = -updir;

        LengthText.transform.position = midpoint + updir * textDistance;
        DataText.transform.position = midpoint - updir * infoDistance;

        AssignLinePointsPositions();

    }

    Vector3 prevFromPos;
    Vector3 prevToPos;

    private void AssignLinePointsPositions()
    {
        if (
            (prevFromPos - From.position).sqrMagnitude < 1e-3 &&
            (prevToPos - To.position).sqrMagnitude < 1e-3)
            return;

        UpdatePoints();
    }

    private void UpdatePoints()
    {
        LineRenderer.SetPositions(points.Select(t => Vector3.Lerp(From.position, To.position, t)).ToArray());
        EdgeCollider.SetPoints(points.Select(t => Vector2.Lerp(From.localPosition, To.localPosition, t)).ToList());

        prevFromPos = From.position;
        prevToPos = To.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((eventData.pressPosition - eventData.position).sqrMagnitude < 2)
            UINodePanelSpawner.Instance.SpawnPanel(GraphicInstance);
    }
}
