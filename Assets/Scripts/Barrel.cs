using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject brokenBarrelPrefab;
    public float destroyDelay = 0.1f; // Pequeno delay antes de destruir

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o barril foi atingido pelo jogador ou pela kunai
        if (collision.CompareTag("PlayerAttack") || collision.CompareTag("Kunai"))
        {
            BreakBarrel();
        }
    }

    void BreakBarrel()
    {
        // Instancia o barril quebrado se houver um prefab para isso
        if (brokenBarrelPrefab != null)
        {
            Instantiate(brokenBarrelPrefab, transform.position, transform.rotation);
        }

        // Destroi o barril original
        Destroy(gameObject, destroyDelay);
    }
}
