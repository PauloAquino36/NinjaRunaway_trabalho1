using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    public BoxCollider2D colliderOriginal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderOriginal = GetComponent<BoxCollider2D>();
        Vector2 newSize = new Vector2(colliderOriginal.size.x * 0.45f, colliderOriginal.size.y);
        colliderOriginal.size = newSize; 
    }
 
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            player.GetComponent<PlayerControl>().animator.SetBool("climb", true);
        }
        
    }

    void OnTriggerExit2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            player.GetComponent<PlayerControl>().animator.SetBool("climb", false);
            player.GetComponent<PlayerControl>().rb.gravityScale = player.GetComponent<PlayerControl>().originalGravityScale;
        }
    }

}
