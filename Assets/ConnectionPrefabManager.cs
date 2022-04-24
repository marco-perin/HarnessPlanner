using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]

public class ConnectionPrefabManager : MonoBehaviourGraphicInstanced, IPointerClickHandler// IPointerEnterHandler, IPointerExitHandler
{
    public Transform From;
    public Transform To;
    public TMP_Text LengthText;
    public EdgeCollider2D EdgeCollider;
    public LineRenderer LineRenderer;

    [Range(2, 200)]
    public int pointNumber = 100;
    public static IEnumerable<float> points;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(From != null);
        Debug.Assert(To != null);
        Debug.Assert(LengthText != null);

        if (EdgeCollider == null)
            EdgeCollider = GetComponent<EdgeCollider2D>();

        if (LineRenderer == null)
            LineRenderer = GetComponent<LineRenderer>();

        Debug.Assert(pointNumber >= 2);
        points = Enumerable.Range(0, pointNumber + 1).Select(n => (float)n / (float)pointNumber);
        LineRenderer.positionCount = pointNumber + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (EdgeCollider == null) return;
        if (LineRenderer == null) return;
        if (LengthText == null) return;

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

        LengthText.transform.position = Vector3.Lerp(From.position, To.position, 0.5f) + Quaternion.FromToRotation(Vector3.right, To.position - From.position) * Vector3.up * 0.2f;
        LineRenderer.SetPositions(points.Select(t => Vector3.Lerp(From.position, To.position, t)).ToArray());
        EdgeCollider.SetPoints(points.Select(t => Vector2.Lerp(From.position, To.position, t)).ToList());
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    //Cursor.SetCursor(null,Cursor.)
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    throw new System.NotImplementedException();
    //}

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
