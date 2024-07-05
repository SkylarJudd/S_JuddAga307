using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<GameObject> OnEnermyHit = null;
    public static event Action<GameObject> OnEnermyDie = null;

    public static void reportOnEnemyHit(GameObject go)
    {
        OnEnermyHit?.Invoke(go);
    }

    public static void reportOnEnemyDie(GameObject go)
    {
        OnEnermyDie?.Invoke(go);
    }
}
