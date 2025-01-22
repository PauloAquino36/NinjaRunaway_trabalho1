using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public Transform playerCamera;
    public float smoothSpeed = 0.125f;
    public GameObject player;
    private SpriteRenderer spriteRenderer;
    private Vector3 currentOffset;
    private float targetZ; // Armazena a posição desejada no eixo Z

    void Start()
    {
        player = GameObject.Find("Player");
        if (player != null)
        {
            spriteRenderer = player.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogError("Player não encontrado na cena!");
        }

        // Define o offset inicial e o Z inicial
        currentOffset = new Vector3(4.5f, 0, playerCamera.position.z);
        targetZ = transform.position.z;
    }

    void FixedUpdate()
    {
        if (spriteRenderer != null)
        {
            // Calcula o offset alvo com base na direção do personagem
            Vector3 targetOffset = spriteRenderer.flipX
                ? new Vector3(-4.5f, 0, playerCamera.position.z) // Offset para a esquerda
                : new Vector3(4.5f, 0, playerCamera.position.z); // Offset para a direita

            currentOffset = Vector3.Lerp(currentOffset, targetOffset, smoothSpeed);

            Vector3 desiredPosition = player.transform.position + currentOffset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        }

        if (player.GetComponent<PlayerControl>().animator.GetBool("glider") || player.GetComponent<PlayerControl>().animator.GetBool("climbMove"))
        {
            targetZ = -5;
        }
        else
        {
            targetZ = 0;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, targetZ, smoothSpeed));
    }
}
