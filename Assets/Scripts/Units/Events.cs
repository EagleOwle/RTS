using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventChangeHealth : UnityEvent<int>
{

}

public class EventSetTarget : UnityEvent<Squad>
{

}

public class EventSetUnit : UnityEvent<Unit>
{

}

public class EventSetBool : UnityEvent<bool>
{

}