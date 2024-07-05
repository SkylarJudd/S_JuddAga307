using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class Enemy : GameBehaviour
{
    [SerializeField] EnemyType MyType;
    [SerializeField] float moveDistance = 1000f;

    [SerializeField] PotrolType myPatrol; // What Petrol Type we are

    [SerializeField] Transform moveToPos; // Needed for all Patrol 
    Transform startPos; //Needed for Loop patrol Movement
    Transform endPos; //Needed for Loop patrol Movement

    bool reverse; //Needed for Loop patrol Movement
    private int PatrolPoint; //Needed for Linear Patrol Movement 


    [SerializeField] float stopDisctance = 0.3f;

    [SerializeField] float myspeed = 5.0f;
    [SerializeField] int myHelth = 100;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H)) 
        {
            EnemyHit(-10);
        }
    }

    public void setup(Transform _startPo)
    {
        

        switch (MyType)
        {
            case EnemyType.OneHanded:
                myspeed = 5f;
                myHelth = 100;
                myPatrol = PotrolType.Random;
                break;
            case EnemyType.TwoHanded:
                myspeed = 3f;
                myHelth = 200;
                myPatrol = PotrolType.Loop;
                break;
            case EnemyType.Archer:
                myspeed = 7f;
                myHelth = 75;
                myPatrol = PotrolType.Linear;
                break;
        }

        startPos = _startPo;
        endPos = _EM.GetRandomSpawnPos();
        while (endPos == startPos)
        {
            endPos = _EM.GetRandomSpawnPos();
        }

        moveToPos = endPos;

        StartCoroutine(MoveEnemy());

        #region OldSetUp
        //if (MyType == EnemyType.OneHanded) { myspeed = 5f; }
        //else if (MyType == EnemyType.TwoHanded) { myspeed = 3f; }
        //else if (MyType == EnemyType.Archer) { myspeed = 7f; }
        #endregion
    }

    private IEnumerator MoveEnemy()
    {
        switch (myPatrol)
        {
            case PotrolType.Linear:
                moveToPos = _EM.GetSpawnPoint(PatrolPoint);
                PatrolPoint = PatrolPoint != _EM.GetSpawnPointsCount() ? PatrolPoint + 1 : 0;
                break;

            case PotrolType.Loop:
                moveToPos = reverse ? startPos : endPos;
                reverse = !reverse;
                break;

            case PotrolType.Random:
                moveToPos = _EM.GetRandomSpawnPos();
                break;
        }
        transform.LookAt(moveToPos);

        while (Vector3.Distance(transform.position, moveToPos.position) > stopDisctance)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveToPos.position, Time.deltaTime * myspeed);
            yield return null;
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(MoveEnemy());
    }

    public void EnemyHit(int _damage)
    {
        myHelth -= _damage;
        print($"health = {myHelth}");

        if (myHelth <= 0)
        {
            EnemyDie();
        }
        else
        {
            GameEvents.reportOnEnemyHit(gameObject);
        }
        
    }

    private void EnemyDie()
    {
        if (myHelth <= 0)
        {
            StopAllCoroutines();
            print("Enermy Dies");
            GameEvents.reportOnEnemyDie(gameObject);
        }
    }
}
