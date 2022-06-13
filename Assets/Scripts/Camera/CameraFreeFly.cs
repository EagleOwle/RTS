using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CameraFreeFly : MonoBehaviour
{
    public float heightPercent;
    public float tmp;
    [SerializeField] private bool onlyMouseHide = false;

    [SerializeField] private float moveSpeed = 15;
    [SerializeField] private float scrollSpeed = 15;
    [Header("Высота")]
    [SerializeField] private float maxHeigth = 15, minHeight = 5;
    [Header("Ширина")]
    [SerializeField] private float maxLenght = 95, minLenght = 5;
    [Header("Длина")]
    [SerializeField] private float maxWidth = 95, minWidth = 5;

    [SerializeField] private float minAngleX = 15;
    [SerializeField] private float maxAngleX = 90;
    [SerializeField] private float speed = 10;
    [SerializeField] private Transform target;

    private const string actionMove = "Move";
    private InputAction moveAction;

    private const string actionLook = "Look";
    private InputAction lookAction;

    private bool aceleration;
    private CharacterController characterController;
    private PlayerInput playerInput;
    private Vector3 moveDirection;
    private float upDirection;
    private float lookAngleX;
    

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GameObject.FindObjectOfType<PlayerInput>();
        moveAction = playerInput.currentActionMap.FindAction(actionMove);
        lookAction = playerInput.currentActionMap.FindAction(actionLook);
    }

    private void Update()
    {
        GetInput();
        FreeMove(moveDirection * moveSpeed * Time.deltaTime);
        
    }

    private void LateUpdate()
    {
       // SetLookAngleX(upDirection);
    }

    private Vector3 Clamp(Vector3 original)
    {
        return new Vector3(Mathf.Clamp(original.x, minWidth, maxWidth),
                           Mathf.Clamp(original.y, minHeight, maxHeigth),
                           Mathf.Clamp(original.z, minLenght, maxLenght));

    }

    private void GetInput()
    {
        upDirection = Mouse.current.scroll.ReadValue().y * -scrollSpeed;
        moveDirection = new Vector3(moveDirection.x, upDirection, moveDirection.y);
        Vector2 move = moveAction.ReadValue<Vector2>();
        moveDirection = new Vector3(move.x, moveDirection.y, move.y);
        //aceleration = Input.GetKey(KeyCode.LeftShift);
        
    }

    private void SetLookAngleX(float upDirection)
    {
        heightPercent = (100 / (maxHeigth - minHeight)) * (transform.position.y - minHeight);

        float angle = Mathf.Lerp(target.localEulerAngles.x, heightPercent, speed * Time.deltaTime);
        angle = Mathf.Clamp(angle, minAngleX, maxAngleX);
        target.localEulerAngles = Vector3.right * angle;
    }

    private void FreeMove(Vector3 moveDirection)
    {
        if (onlyMouseHide)
        {
            if (Cursor.visible)
                return;
        }

        if(aceleration)
        {
            moveDirection *= 2;
        }

        characterController.Move(transform.TransformVector(moveDirection));
        transform.position = Clamp(transform.position);
    }

}
