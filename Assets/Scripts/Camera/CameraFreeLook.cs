using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFreeLook : MonoBehaviour
{
    [SerializeField] private bool onlyMouseHide = false;
    [SerializeField] private Transform xRotationTransform;
    [SerializeField] private Transform yRotationTransform;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;

    private float rotationX = 0F;
    private float rotationY = 0F;
    private Quaternion xOriginalRotation;
    private Quaternion yOriginalRotation;

    private void Start()
    {
        xOriginalRotation = xRotationTransform.localRotation;
        yOriginalRotation = yRotationTransform.localRotation;
    }

    void Update()
    {
        if (onlyMouseHide)
        {
            if (Cursor.visible)
                return;
        }

        GetInput();

        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

        xRotationTransform.localRotation = xOriginalRotation * xQuaternion;
        yRotationTransform.localRotation = yOriginalRotation * yQuaternion;
    }

    private void GetInput()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }
}
