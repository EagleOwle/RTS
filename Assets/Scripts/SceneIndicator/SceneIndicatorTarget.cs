using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneIndicatorTarget : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(CreateIndicator), 1);
    }

    private void CreateIndicator()
    {
        //UISceneIndicatorManager.Instance.AddTarget(this);
    }

}
