using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class CameraAdjustmentAngle : MonoBehaviour
{
    [SerializeField] private float minAngleX = 0;
    [SerializeField] private float maxAngleX = 45;
    [SerializeField] private float speed = 10;
    [SerializeField] private Transform target;


    private const string actionLook = "Look";
    private PlayerInput playerInput;
    private InputAction lookAction;

    private void Awake()
    {
        playerInput = GameObject.FindObjectOfType<PlayerInput>();
        playerInput.onActionTriggered += ReadInputAction;
        lookAction = playerInput.currentActionMap.FindAction(actionLook);
    }

    private void ReadInputAction(InputAction.CallbackContext context)
    {
        if (context.action == lookAction)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    break;
                case InputActionPhase.Performed:
                    
                    float rotationX = target.localEulerAngles.x;
                    Debug.Log("Rotation= " + rotationX);
                    rotationX += speed * Time.deltaTime;
                    rotationX = Mathf.Clamp(rotationX, minAngleX, maxAngleX);
                    target.localEulerAngles = Vector3.right * rotationX;
                    break;
                case InputActionPhase.Canceled:
                    break;
            }
        }

    }
}
