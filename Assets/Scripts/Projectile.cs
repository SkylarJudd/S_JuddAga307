using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitParticles;
    [SerializeField] float lifetime = 30f;
    [SerializeField] int damage = 10;
    [SerializeField] int speed = 10;


    Rigidbody _bulletRigidBody;

    private void Awake()
    {
        _bulletRigidBody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        Invoke("DestroyProjectile", lifetime);
        _bulletRigidBody.linearVelocity = transform.forward * speed; 
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
        if (collision.collider.CompareTag("Enemy"))
        {

            if (collision.gameObject.GetComponent<Enemy>() != null)
            {
                collision.gameObject.GetComponent<Enemy>().EnemyHit(damage);
            }
        }
        ChickenEnermy chickenEnermy = collision.gameObject.GetComponent<ChickenEnermy>();
        if (chickenEnermy != null)
        {
            chickenEnermy.ChickenHit(damage);
        }
        else
        {
            print("NotChickenHit");
        }
        DestroyProjectile();
        
    }

}
