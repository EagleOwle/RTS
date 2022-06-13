using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public enum FrendlyState {enemy, friendly }

[System.Serializable]
public class Squad
{
    private SquadHealth squadHealth;

    private FrendlyState frendlyState;
    public FrendlyState FrendlyState => frendlyState;
    private Unit[] units;
    private const string groundLayerMask = "Terrain";
    private LayerMask groundLayer;
    private bool isSelect;
    public bool IsSelect
    {
        set
        {
            if (isSelect != value)
            {
                actionSelect?.Invoke(value);
                isSelect = value;
            }
        }
    }

    public Action<bool> actionSelect;

    private PlayerInput playerInput;
    private const string actionMouseButton = "Fire";
    private InputAction mouseButton;
    private InputActionMap gameplayActions;

    public void Initialise(FrendlyState frendlyState, int squadSize = 5)
    {
        this.frendlyState = frendlyState;

        units = new Unit[squadSize];
        squadHealth = new SquadHealth(squadSize);

        if (frendlyState == FrendlyState.friendly)
        {
            groundLayer = LayerMask.GetMask(groundLayerMask);
            playerInput = GameObject.FindObjectOfType<PlayerInput>();
            mouseButton = playerInput.currentActionMap.FindAction(actionMouseButton);
            mouseButton.performed += MouseButton_performed;
        }
    }

    private void MouseButton_performed(InputAction.CallbackContext context)
    {
        if(Mouse.current.leftButton.isPressed)
        {
            IsSelect = false;
        }

        if (Mouse.current.rightButton.isPressed)
        {
            //Debug.Log("RightButton");
            if (isSelect)
            {
                GetWorldPosition(Mouse.current.position.ReadValue());
            }
        }

        
    }

    private void SetDestination(Vector3 worldPosition)
    {
        foreach (var item in units)
        {
            item.Destination(worldPosition);
        }
    }

    private void GetWorldPosition(Vector3 screenPosition)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            SetDestination(hit.point);
        }
    }

    public async void CreateUnit(float seconds)
    {
        await Task.Delay(TimeSpan.FromSeconds(seconds));

        Unit unit = null;
        unit = GameObject.Instantiate(Resources.Load("Prefabs/Unit")) as Unit;
    }

    public async void InstantiateUnits(Vector3 position)
    {
        int repite = units.Length;
        while (repite > 0)
        {
            repite--;
            Unit unit = GameObject.Instantiate(Resources.Load("Prefabs/Unit", typeof(Unit))) as Unit;
            unit.transform.position = position;
            
            await Task.Delay(TimeSpan.FromSeconds(0.1f));

            unit.Initialise(this);
            units[repite] = unit;

            UnitHealth unitHealth = unit.gameObject.AddComponent<UnitHealth>();
            unit.health = unitHealth;
            squadHealth.AddUnit(unitHealth);

            unitHealth.Health = UnityEngine.Random.Range(10, 100);
        }

        SetCapitan(units[0]);
    }

    private void SetCapitan(Unit unit)
    {
        foreach (var item in units)
        {
            item.Capitan = unit;
        }
    }

    public int GetUnitIndex(Unit unit)
    {
        for (int i = 0; i < units.Length; i++)
        {
            if(units[i] == unit)
            {
                return i;

            }
        }

        return -1;
    }

}


