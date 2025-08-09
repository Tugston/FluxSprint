using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    public static float movementSpeed = 5;

    private GameObject m_ParentObject;
    private Vector3 m_Direction = new Vector3(-1, 0, 0);
    private Rigidbody2D rb;

    private void Awake() =>
        rb = gameObject.GetComponent<Rigidbody2D>();

    void Start()
    {
        m_ParentObject = gameObject;
    }

    void FixedUpdate()
    {
        rb.transform.position += m_Direction * movementSpeed * Time.deltaTime;
    }
}
