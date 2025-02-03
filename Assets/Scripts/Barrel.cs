using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject brokenBarrelPrefab;
    public float destroyDelay = 0.5f; // Pequeno delay antes de destruir
    public AudioSource audioSource;
    public AudioClip sound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o barril foi atingido pelo jogador ou pela kunai
        if (collision.CompareTag("PlayerAttack") || collision.CompareTag("Kunai"))
        {
            // Toca o som do barril quebrando
            audioSource.PlayOneShot(sound);
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
