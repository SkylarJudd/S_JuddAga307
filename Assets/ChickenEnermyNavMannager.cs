using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChickenEnermyMannager;

public class ChickenEnermyNavMannager : Singleton<ChickenEnermyNavMannager>
{
    public List<ValidNavPoints> NavPoints;

    [SerializeField] float stopDistance;

    Coroutine navRoutine;
    bool move = true;

    private void Start()
    {
        navRoutine = StartCoroutine(MoveChickenEnemies());
    }

    private IEnumerator MoveChickenEnemies()
    {
        while (move)
        {
            List<spawnnedChickenEnemiesData> EnemiesToMove;
            EnemiesToMove = _CEM.spawnnedChickenEnemies;

            if (_CEM.spawnnedChickenEnemies.Count != 0)
            {
                foreach (spawnnedChickenEnemiesData go in EnemiesToMove)
                {
                    ChickenMove(go);
                    ChickenRotate(go);
                }
            }
            yield return new WaitForEndOfFrame();
        }

    }

    private void ChickenMove(spawnnedChickenEnemiesData go)
    {
        float _distance = Vector3.Distance(go.spawnnedChickenGO.transform.position, go.targetLocation.navPointTransform.transform.position);

        if (_distance < stopDistance)
        {
            go.targetLocation = go.targetLocation.validNavTransforms[UnityEngine.Random.Range(0, go.targetLocation.validNavTransforms.Length)];
        }
        else
        {

            go.spawnnedChickenGO.transform.position = (Vector3.MoveTowards(go.spawnnedChickenGO.transform.position, go.targetLocation.navPointTransform.transform.position, Time.deltaTime * go.chickenSpeed * go.alignmentFactor));

        }
    }

    private void ChickenRotate(spawnnedChickenEnemiesData go)
    {
        Vector3 aimDirection = (go.spawnnedChickenGO.transform.position - go.targetLocation.navPointTransform.transform.position).normalized;


        float alignment = Vector3.Dot(go.spawnnedChickenGO.transform.forward, aimDirection);


        float alignmentFactor = (alignment + 1) / 2;

        go.spawnnedChickenGO.transform.forward = Vector3.Lerp(go.spawnnedChickenGO.transform.forward, aimDirection, Time.deltaTime * go.chickenRotateSpeed);

        //Debug.Log("Alignment Factor: " + alignmentFactor);
        go.alignmentFactor = alignmentFactor;
    }

    public void StopMovement()
    {
        move = false;
        StopCoroutine(navRoutine);

    }

    public void StartMovement()
    {
        move = true;
        navRoutine = StartCoroutine(MoveChickenEnemies());
    }

}
