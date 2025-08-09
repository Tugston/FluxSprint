using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillVolume : MonoBehaviour
{

    private GameObject m_Player;
    private BoxCollider2D m_Collision;
    
    void Start()
    {
        m_Collision = gameObject.GetComponent<BoxCollider2D>();
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D overlap)
    {
        if(overlap.gameObject.tag == "Player")
        {
            RestartLevel();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            RestartLevel();
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
