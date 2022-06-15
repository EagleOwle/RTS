using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneIndicator : MonoBehaviour
{
    [SerializeField] protected string cameraName = "Main Camera";
    [SerializeField] protected string parentName = "GameCanvas";
    [SerializeField] protected Vector3 offset;

    [SerializeField] protected float maxAngle;

    [SerializeField] protected GameObject[] indicatorArrayObj;

    private Squad squad;
    public Squad Squad => squad;

    private GameObject Target
    {
        get
        {
            if (squad.Capitan == null) return null;
            
            return squad.Capitan.gameObject;
        }
    }

    private bool visible = false;
    protected bool Visible
    {
        get
        {
            return visible;
        }
        set
        {
            visible = value;

            foreach (GameObject obj in indicatorArrayObj)
            {
                obj.SetActive(value);
            }

        }
    }

    protected Camera currentCamera;
    protected RectTransform rectTarnsform;

    protected virtual void Awake()
    {
        rectTarnsform = GetComponent<RectTransform>();
    }

    protected virtual void OnEnable()
    {
        if(transform.parent == null)
        {
            transform.parent = GameObject.Find(parentName).transform;
        }

        foreach (GameObject item in indicatorArrayObj)
        {
            item.SetActive(false);
        }
    }

    protected virtual void LateUpdate()
    {
        Visible = true;

        #region Удаляемся, если нет обьекта-цели
        if (Target == null)
        {
            Visible = false;
            //Destroy(gameObject);
            return;
        }
        #endregion

        #region Ищем камеру на сцене
        if (currentCamera == null)
        {
            if (GameObject.Find(cameraName) != null)
            {
                GameObject tmp = GameObject.Find(cameraName);
                currentCamera = tmp.GetComponent<Camera>();
            }
            else
            {
                Visible = false;
                return;
            }
        }
        #endregion

        #region Проверяем угол
        if (CheckAngle() == false)
        {
            Visible = false;
            return;
        }
        #endregion

        #region Задаем позицию на экране
        rectTarnsform.position = currentCamera.WorldToScreenPoint(Target.transform.position + offset);
        #endregion

    }

    protected virtual bool CheckAngle()
    {
        if (currentCamera == null)
        {
            return false;
        }

        if (Target == null)
        {
            return false;
        }

        Vector3 to = currentCamera.transform.forward;
        Vector3 from = Target.transform.position - currentCamera.transform.position;

        if (Vector3.Angle(from, to) > maxAngle)
        {
            return false;
        }

        return true;
    }

    public void SetTarget(Squad value)
    {
        squad = value;

        foreach (GameObject item in indicatorArrayObj)
        {
            item.SetActive(true);
        }

    }


}
