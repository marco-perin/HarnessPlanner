using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDraggable : MonoBehaviour
{

    public TransformDraggable canvasTransformDraggable;

    public float moveSpeed = 1;

    void Start()
    {
        InputManager.Instance.AddKeyDownAction(KeyCode.LeftArrow, () => MoveHoriz(-1), true);
        InputManager.Instance.AddKeyDownAction(KeyCode.RightArrow, () => MoveHoriz(1), true);
        InputManager.Instance.AddKeyDownAction(KeyCode.DownArrow, () => MoveVert(-1), true);
        InputManager.Instance.AddKeyDownAction(KeyCode.UpArrow, () => MoveVert(1), true);
    }

    private void MoveVert(float v)
    {
        Move(3 * moveSpeed * v * Vector3.up);
    }

    private void MoveHoriz(float v)
    {

        Move(3 * moveSpeed * v * Vector3.right);
    }

    private void Move(Vector3 movement)
    {
        canvasTransformDraggable.TargetPosition -= movement;
    }
}
