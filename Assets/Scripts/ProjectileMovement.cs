using System.Threading;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float m_ProjectileSpeed = 75.0f;

    private Rigidbody2D m_RigidBody;
    [SerializeField] private ParticleSystem m_Particles;

    private void Awake() =>
        m_RigidBody = GetComponent<Rigidbody2D>();

    private void Start()
    {
        Invoke("DestroyProjectile", 1.0f);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = m_RigidBody.transform.position + (m_RigidBody.transform.up * m_ProjectileSpeed * Time.deltaTime);
        m_RigidBody.MovePosition(velocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate<ParticleSystem>(m_Particles, collision.transform.position, collision.transform.rotation);
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
