using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChickenEnermyMannager;

public class ChickenEnermyNavMannager : Singleton<ChickenEnermyNavMannager>
{
    public List<ValidNavPoints> NavPoints;

    [SerializeField] float stopDistance;

    private IEnumerator MoveChickenEnemies()
    {
        if (_CEM.spawnnedChickenEnemies.Count != 0)
        {
            foreach (spawnnedChickenEnemiesData go in _CEM.spawnnedChickenEnemies)
            {
                ChickenMove(go);
                ChickenRotate(go);
            }
        }
        yield return new WaitForEndOfFrame();
    }

    private void ChickenMove(spawnnedChickenEnemiesData go)
    {
        float _distance = Vector3.Distance(go.spawnnedChickenGO.transform.position, go.targetLocation.transform.position);

        if (_distance < stopDistance)
        {
            //set a new target pos
        }
        else
        {
            transform.position = Vector3.MoveTowards(go.spawnnedChickenGO.transform.position, go.targetLocation.transform.position, Time.deltaTime * go.chickenSpeed);
        }
    }

    private void ChickenRotate(spawnnedChickenEnemiesData go)
    {
        Vector3 aimDirection = (go.spawnnedChickenGO.transform.position - go.targetLocation.transform.position).normalized;

        go.spawnnedChickenGO.transform.forward = Vector3.Lerp(go.spawnnedChickenGO.transform.forward , aimDirection, Time.deltaTime * go.chickenRotateSpeed);
    }
}
