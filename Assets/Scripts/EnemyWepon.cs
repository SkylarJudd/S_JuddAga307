using UnityEngine;

public class EnemyWepon : GameBehaviour
{
    [SerializeField] int damage = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("PlayerHit");
            _PLAYER.Hit(damage);
        }
    }
}
