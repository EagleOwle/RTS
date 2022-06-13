using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneIndicatorManager : MonoBehaviour
{
    private static UISceneIndicatorManager _instance;
    public static UISceneIndicatorManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UISceneIndicatorManager>();
            }

            return _instance;
        }
    }

    [SerializeField] private UISceneIndicator sceneIndicatorPrefab;

    public void AddTarget(SceneIndicatorTarget sceneIndicatorTarget)
    {
        UISceneIndicator sceneIndicator = GameObject.Instantiate(Resources.Load("Prefabs/UISceneIndicator", typeof(UISceneIndicator))) as UISceneIndicator;
        sceneIndicator.transform.SetParent(transform);
        sceneIndicator.SetTarget(sceneIndicatorTarget.gameObject);
    }

    public void AddTarget(GameObject sceneIndicatorTarget)
    {
        UISceneIndicator sceneIndicator = GameObject.Instantiate(Resources.Load("Prefabs/UISceneIndicator", typeof(UISceneIndicator))) as UISceneIndicator;
        sceneIndicator.transform.SetParent(transform);
        sceneIndicator.SetTarget(sceneIndicatorTarget.gameObject);
    }

}
