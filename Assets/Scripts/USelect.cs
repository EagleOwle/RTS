using UnityEngine;

public class USelect : MonoBehaviour
{
    public bool Selected = false;
    public bool FindUnit = false;

    public Texture BoxSelect = null;

    private float _width = 0.0f;
    private float _height = 0.0f;

    private Vector3 _startPoint = Vector3.zero;
    private Vector3 _endPoint = Vector3.zero;

    private Rect s_rect;

    //private PController _pcontroller = null;
    private Camera m_camera = null;

    private void Start()
    {
        //_pcontroller = GetComponent<PController>();
        m_camera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _startPoint = Input.mousePosition;
            //_pcontroller.selectGroup.Clear();
            Selected = true;
        }
        if(Selected)
        {
            _endPoint = Input.mousePosition;
            if(Input.GetMouseButtonUp(0))
            {
                s_rect = SelectRect(_startPoint, _endPoint);
                FindUnit = true;
                Selected = false;
            }
        }else if(FindUnit)
        {
            //foreach(GameObject tmp in _pcontroller.gameUnits)
            //{
            //    var pos = m_camera.WorldToScreenPoint(tmp.transform.position);
            //    pos.y = InvertY(pos.y);
            //    if (s_rect.Contains(pos))
            //    {
            //        _pcontroller.selectGroup.Add(tmp);
            //    }
            //}
            FindUnit = false;
        }
    }

    private void OnGUI()
    {
        if(Selected)
        {
            _width = _endPoint.x - _startPoint.x;
            _height = InvertY(_endPoint.y) - InvertY(_startPoint.y);
            GUI.DrawTexture(new Rect(_startPoint.x, InvertY(_startPoint.y), _width, _height), BoxSelect);
        }
    }

    private Rect SelectRect(Vector3 _start, Vector3 _end)
    {
        if (_width < 0.0f)
        {
            _width = Mathf.Abs(_width);
        }
        if (_height < 0.0f)
        {
            _height = Mathf.Abs(_height);
        }

        if (_endPoint.x < _startPoint.x)
        {
            _start.z = _start.x;
            _start.x = _end.x;
            _end.x = _start.z;
        }
        if (_endPoint.y > _startPoint.y)
        {
            _start.z = _start.y;
            _start.y = _end.y;
            _end.y = _start.z;
        }

        return new Rect(_start.x, InvertY(_start.y), _width, _height);
    }

    private float InvertY(float _y)
    {
        return Screen.height - _y;
    }
}
