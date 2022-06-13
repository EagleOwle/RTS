using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SquadSelect : MonoBehaviour
{
    private static SquadSelect instance;
    public static SquadSelect Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<SquadSelect>();
            }

            return instance;
        }
    }

    private List<Unit> readyForChange = new List<Unit>();

    public bool selected = false;
    public Texture BoxSelect = null;

    private Vector3 startPoint = Vector3.zero;
    private Vector3 endPoint = Vector3.zero;

    private Rect rect;
    private float width = 0.0f;
    private float height = 0.0f;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startPoint = Mouse.current.position.ReadValue();
            selected = true;
        }

        if (selected)
        {
            endPoint = Mouse.current.position.ReadValue();
            rect = SelectRect(startPoint, endPoint);
            FindUnits();

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                selected = false;
            }
        }
    }

    private void FindUnits()
    {
        foreach (Unit tmp in readyForChange)
        {
            var pos = Camera.main.WorldToScreenPoint(tmp.transform.position);
            pos.y = InvertY(pos.y);

            if (rect.Contains(pos))
            {
                tmp.Squad.IsSelect = true;
            }
        }
    }

    public void AddReadyForChange(Unit unit)
    {
        foreach (var item in readyForChange)
        {
            if (item == unit) return;
        }

        readyForChange.Add(unit);
    }

    public void RemoveReadyForChange(Unit unit)
    {
        for (int i = 0; i < readyForChange.Count; i++)
        {
            if (readyForChange[i] == null)
            {
                readyForChange.RemoveAt(i);
                continue;
            }

            if (readyForChange[i] == unit)
            {
                readyForChange.RemoveAt(i);
            }
        }
    }

    private Rect SelectRect(Vector3 _start, Vector3 _end)
    {
        if (width < 0.0f)
        {
            width = Mathf.Abs(width);
        }
        if (height < 0.0f)
        {
            height = Mathf.Abs(height);
        }

        if (endPoint.x < startPoint.x)
        {
            _start.z = _start.x;
            _start.x = _end.x;
            _end.x = _start.z;
        }
        if (endPoint.y > startPoint.y)
        {
            _start.z = _start.y;
            _start.y = _end.y;
            _end.y = _start.z;
        }

        return new Rect(_start.x, InvertY(_start.y), width, height);
    }

    private void OnGUI()
    {
        if (selected)
        {
            width = endPoint.x - startPoint.x;
            height = InvertY(endPoint.y) - InvertY(startPoint.y);
            GUI.DrawTexture(new Rect(startPoint.x, InvertY(startPoint.y), width, height), BoxSelect);
        }
    }

    private float InvertY(float y)
    {
        return Screen.height - y;
    }

}
