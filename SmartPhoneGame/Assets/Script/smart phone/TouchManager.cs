using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager{

    public enum Side { Left, Right};

    public bool HasTouch { get; private set; }
    public int TouchCount { get; private set; }
    public Vector2[] TouchPosition { get; private set; }
    public TouchPhase[] PhaseTouch { get; private set; }
    public Vector2[] Direction { get; private set; }
    public Side[] side { get; private set; }

    private const int maxTouch = 2;

    private Vector2[] oldPosition;

    public TouchManager()
    {
        HasTouch = false;
        TouchPosition = new Vector2[maxTouch];
        PhaseTouch = new TouchPhase[maxTouch];
        Direction = new Vector2[maxTouch];
        side = new Side[maxTouch];
        oldPosition = new Vector2[maxTouch];

        for (int i = 0; i < maxTouch; i++)
        {
            TouchPosition[i] = Vector2.zero;
            PhaseTouch[i] = TouchPhase.Canceled;
            oldPosition[i] = Vector2.zero;
        }
    }

    public void Update()
    {
        HasTouch = false;

        if (Application.isEditor)
        {
            //if (Input.GetMouseButton(0))
            //{
            //    PhaseTouch[0] = TouchPhase.Moved;
            //    HasTouch = true;
            //}

            //if (Input.GetMouseButtonDown(0))
            //{
            //    PhaseTouch[0] = TouchPhase.Began;
            //    HasTouch = true;
            //}

            //if (Input.GetMouseButtonUp(0))
            //{
            //    PhaseTouch[0] = TouchPhase.Ended;
            //    HasTouch = true;
            //}

            //if (HasTouch)
            //{
            //    TouchCount = 1;
            //    TouchPosition[0] = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //    SetSide();
            //}
            //if (PhaseTouch[0] == TouchPhase.Began)
            //    oldPosition[0] = TouchPosition[0];
        }
        else
        {
            TouchCount = Input.touchCount;

            if (TouchCount <= 0)
                return;

            Touch[] touch = new Touch[TouchCount];
            HasTouch = true;
            for (int i = 0; i < touch.Length; i++)
            {
                touch[i] = Input.GetTouch(i);
                TouchPosition[i] = Camera.main.ScreenToWorldPoint(touch[i].position);
                PhaseTouch[i] = touch[i].phase;
                SetSide(i);
            }
        }
        for (int i = 0; i < TouchCount; i++)
        {
            Direction[i] = CalcDirection(i);
            oldPosition[i] = TouchPosition[i];
        }
    }

    public TouchManager GetTouch()
    {
        return new TouchManager();
    }

    private void SetSide(int i)
    {
        if (TouchPosition[i].x > 0)
            side[i] = Side.Right;
        else
            side[i] = Side.Left;
    }

    private void SetSide()
    {
        if (TouchPosition[0].x > 0)
            side[0] = Side.Right;
        else
            side[0] = Side.Left;
    }


    private Vector2 CalcDirection(int i)
    {
        return (TouchPosition[i] - oldPosition[i]).normalized;
    }
}
