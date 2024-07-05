using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject hitParticles;
    public float lifetime = 3f;

    public int damage = 10;
    void Start()
    {
        Invoke("DestroyProjectile", lifetime);
    }

    public void DestroyProjectile()
    {
        GameObject hitParticlesInstance = Instantiate(hitParticles, transform.position, transform.rotation);
        Destroy(hitParticlesInstance, 2);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print(collision.gameObject.name);
        if(collision.collider.CompareTag("Enemy"))
        {
 
            if(collision.gameObject.GetComponent<Enemy>() != null)
            {
                collision.gameObject.GetComponent<Enemy>().EnemyHit(damage);
            }
        }
        DestroyProjectile();
        
    }

}
