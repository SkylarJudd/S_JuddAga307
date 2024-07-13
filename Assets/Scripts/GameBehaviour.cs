using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GameBehaviour : MonoBehaviour
{
    protected static GameMannager _GM { get { return GameMannager.instance; } }
    protected static EnemyMannager _EM { get { return EnemyMannager.instance; } }

    protected static ChickenEnermyMannager _CEM { get { return ChickenEnermyMannager.instance; } }
    protected static ChickenEnermyNavMannager _CNM { get { return ChickenEnermyNavMannager.instance; } }

    public Transform getClosestEnermy(Transform _origin, List<GameObject> _objects)
    {
        if (_objects == null || _objects.Count == 0)
            return null;

        float distance = Mathf.Infinity;
        Transform closest = null;

        foreach (GameObject go in _objects)
        {
            float currentDistance = Vector3.Distance(_origin.transform.position, go.transform.position);
            if (currentDistance < distance)
            {
                closest = go.transform;
                distance = currentDistance;
            }
        }
        return closest;
    }

    public Transform getClosestEnermy(Transform _origin, List<ValidNavPoints> _objects)
    {
        if (_objects == null || _objects.Count == 0)
            return null;

        float distance = Mathf.Infinity;
        Transform closest = null;

        foreach (ValidNavPoints go in _objects)
        {
            float currentDistance = Vector3.Distance(_origin.transform.position, go.transform.position);
            if (currentDistance < distance)
            {
                closest = go.transform;
                distance = currentDistance;
            }
        }
        return closest;
    }
}
