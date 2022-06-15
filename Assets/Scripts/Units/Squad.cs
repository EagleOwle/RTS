using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public enum FrendlyState {enemy, friendly }

[System.Serializable]
public class Squad
{
    private UnitState squadState;
    public UnitState SquadState => squadState;

    private SquadHealth squadHealth;
    public SquadHealth SquadHealth => squadHealth;

    private FrendlyState frendlyState;
    public FrendlyState FrendlyState => frendlyState;
    private Unit capitan;
    public Unit Capitan
    {
        set
        {
            capitan = value;
            squadPattern.transform.SetParent(capitan.transform);
            squadPattern.transform.localPosition = Vector3.zero;
            squadPattern.transform.localEulerAngles = Vector3.zero;
            eventSetCapitan.Invoke(capitan);
        }

        get
        {
            return capitan;
        }
    }

    public SquadPattern squadPattern;

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
                eventSelect.Invoke(value);
                isSelect = value;
            }
        }
    }

    public EventSetUnit eventSetCapitan = new EventSetUnit();
    public EventSetBool eventSelect = new EventSetBool();
    public EventSetTarget eventSetTarget = new EventSetTarget();

    private PlayerInput playerInput;
    private const string actionMouseButton = "Fire";
    private InputAction mouseButton;
    private InputActionMap gameplayActions;

    public Transform[] spawnPoint;

    public void Initialise(FrendlyState frendlyState, string namePrefab, Transform[] spawnPoint, int squadSize)
    {
        this.frendlyState = frendlyState;
        this.spawnPoint = spawnPoint;

        units = new Unit[squadSize];
        squadHealth = new SquadHealth(squadSize);

        Vector3 spawnPosition = Vector3.zero;

        if (frendlyState == FrendlyState.friendly)
        {
            groundLayer = LayerMask.GetMask(groundLayerMask);
            playerInput = GameObject.FindObjectOfType<PlayerInput>();
            mouseButton = playerInput.currentActionMap.FindAction(actionMouseButton);
            mouseButton.performed += MouseButton_performed;
            spawnPosition = Player.Instance.transform.position;
        }
        else
        {
            spawnPosition = Enemy.Instance.transform.position;
        }

        squadPattern = GameObject.Instantiate(Resources.Load("Prefabs/SquadPattern", typeof(SquadPattern))) as SquadPattern;

        InstantiateUnits(spawnPosition, namePrefab);
        InstantiateSceneIndicator(this);

        StartGame(1);

    }

    private void MouseButton_performed(InputAction.CallbackContext context)
    {
        if (Mouse.current.leftButton.isPressed)
        {
            IsSelect = false;
        }

        if (Mouse.current.rightButton.isPressed)
        {
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
            if (item == null) continue; 

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

    public async void StartGame(float seconds)
    {
        await Task.Delay(TimeSpan.FromSeconds(seconds));
        squadState = UnitState.Ready;
        SearchEnemy();
    }

    private async void InstantiateUnits(Vector3 position, string prefabName = "Unit")
    {
        int repite = units.Length;
        while (repite > 0)
        {
            repite--;
            Unit unit = GameObject.Instantiate(Resources.Load("Prefabs/" + prefabName, typeof(Unit)),position, Quaternion.identity ) as Unit;

            unit.Initialise(this);
            units[repite] = unit;

            squadHealth.AddUnit(unit.Health);
            unit.Health.Initialise();

            await Task.Delay(TimeSpan.FromSeconds(0.5f));
        }

        Capitan = units[0];
    }

    public void OnUnitDestroy(Unit unit)
    {
        units = units.OrderBy(x => x.UnitState).ToArray();

        if(units[0].UnitState == UnitState.Dead)
        {
            OnDestroy();
            return;
        }

        Capitan = units[0];
    }

    public int GetUnitIndex(Unit unit)
    {
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i] == unit)
            {
                return i;

            }
        }

        return -1;
    }

    private void InstantiateSceneIndicator(Squad target)
    {
        UISceneIndicator sceneIndicator = GameObject.Instantiate(Resources.Load("Prefabs/UISceneIndicator", typeof(UISceneIndicator))) as UISceneIndicator;
        sceneIndicator.SetTarget(target);
    }

    public Vector3 GetWorldPosition()
    {
        if (Capitan == null)
        {
            return Vector3.zero;
        }
        else
        {
            return Capitan.transform.position;
        }
    }

    private async void SearchEnemy()
    {
        while (true)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));

            List<Squad> squads = new List<Squad>();

            switch (frendlyState)
            {
                case FrendlyState.enemy:
                    squads = Player.Instance.Squads.ToList();
                    break;
                case FrendlyState.friendly:

                    squads = Enemy.Instance.Squads.ToList();
                    break;
            }

            for (int i = 0; i < squads.Count; i++)
            {
                if (squads[i].SquadState == UnitState.Dead)
                {
                    squads[i] = null;
                }
            }

            squads.RemoveAll(x => x == null);

            if (squads.Count > 0)
            {
                squads = squads.OrderBy((d) => (d.GetWorldPosition() - GetWorldPosition()).magnitude).ToList();

                if ((squads[0].GetWorldPosition() - GetWorldPosition()).magnitude <= 25)
                {
                    eventSetTarget.Invoke(squads[0]);
                }

                if (frendlyState == FrendlyState.enemy)
                {
                    SetDestination(squads[0].GetWorldPosition());
                }
            }
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < units.Length; i++)
        {
           GameObject.Destroy(units[i].gameObject);
        }

        squadState = UnitState.Dead;

        if (FrendlyState == FrendlyState.friendly)
        {
            Player.Instance.SquadDestroy(this);
        }
        else
        {
            Enemy.Instance.SquadDestroy(this);
        }
    }

}


