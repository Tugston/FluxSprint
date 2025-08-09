using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public float jumpForce = 300.0f;
    public float secondJumpForce = 450.0f;
    public int maxJumps = 2; //max jumps before hitting the ground
    public bool onGround = true;

    private InputActionMap m_InputActions;
    private Rigidbody2D m_RigidBody;
    private int m_JumpCount = 0;
    [SerializeField] private GameObject m_Projectile;
    [SerializeField] private GameObject m_LaserGun;
    
    private float m_AimAngle;
    private Vector3 m_MouseWorldPos;
    private Vector3 m_AimDirection;
    private Vector3 m_ProjectileSpawnLocation;

    private bool m_GunFacingRight = true;

    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_ProjectileSpawnLocation = transform.position + new Vector3(0.25f, 0.0f, 0.0f);
    }
        
        

    void Start()
    {
    }

    private void FixedUpdate()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        mouseScreenPos.z = MathF.Abs(Camera.main.transform.position.z);
        m_MouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        //grab the directionional tangent in degrees
        m_AimDirection = (m_MouseWorldPos - m_ProjectileSpawnLocation).normalized;
        m_AimAngle = Mathf.Atan2(m_AimDirection.y, m_AimDirection.x) * Mathf.Rad2Deg;

        Quaternion gunAngle = Quaternion.Euler(0, 0, m_AimAngle);
        m_LaserGun.GetComponent<Rigidbody2D>().SetRotation(gunAngle);

        if(m_GunFacingRight && m_AimAngle >= 90)
            FlipGun();
        else if(!m_GunFacingRight && m_AimAngle >= -90 && m_AimAngle < 0)
            FlipGun();
    }

    private void FlipGun()
    {
        m_GunFacingRight = !m_GunFacingRight;
        Vector3 newScale = m_LaserGun.transform.localScale;
        newScale.y *= -1;
        m_LaserGun.transform.localScale = newScale;
    }

    public void OnJump(InputValue value)
    {
        if(m_JumpCount < maxJumps)
        {
            m_RigidBody.AddForceY(m_JumpCount == 0 ? jumpForce : secondJumpForce);
            m_JumpCount++;
            onGround = false;
        }
    }

    private Coroutine m_FireRoutine;
    public float firerate = 0.5f;

    public void OnShoot(InputValue value)
    {
        if (value.isPressed)
        {
            //check if there is a leftover routine to prevent bugs
            if(m_FireRoutine != null)
            {
                StopCoroutine(m_FireRoutine);
            }

            m_FireRoutine = StartCoroutine(RapidFireRoutine());
        } else
        {
            StopCoroutine(m_FireRoutine);
        }
    }

    private IEnumerator RapidFireRoutine()
    {
        while(true)
        {
            SpawnProjectile();
            yield return new WaitForSeconds(firerate);
        }
    }

    private void SpawnProjectile()
    { 
        Quaternion aimRotation = Quaternion.Euler(0, 0, m_AimAngle - 90f);
        Instantiate(m_Projectile, transform.position, aimRotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            m_JumpCount = 0;
            onGround = true;
        }
    }
}
