using UnityEngine;
using UnityEngine.UI;

public class ChickenEnermy : MonoBehaviour
{
    [Header("ChickenInfo")]
    [SerializeField] float myspeed = 5.0f;
    [SerializeField] int myHealth = 100;
    [SerializeField] Slider myHealthSlider;

    public ChickenEnemySO chickenEnemySO;

    public void setup(Transform _startPos)
    {
        myHealthSlider.maxValue = myHealth;
    }

    public void ChickenHit(int _damage)
    {
        myHealth -= _damage;
        print($"health = {myHealth}");
        myHealthSlider.value = myHealth;

        if (myHealth <= 0)
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
        if (myHealth <= 0)
        {
            StopAllCoroutines();
            print("Enermy Dies");
            GameEvents.reportOnEnemyDie(gameObject);
        }
    }
}
