using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();         // get rigid body

        if (!rb)                                // if there's no rigid body
        {
            rb = gameObject.AddComponent<Rigidbody>();  // then add rigid body
        }

        rb.useGravity = false; // no gravity
        rb.isKinematic = false; // ensure physics works
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // prevent passing through objects

        // move projectile forward
        rb.linearVelocity = transform.forward * speed;

        // destroy projectile after time
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // if it collides with an enemy
        {
            Debug.Log("Hit enemy!");
            Destroy(gameObject);            // destroy projectile
        }
    }
}
