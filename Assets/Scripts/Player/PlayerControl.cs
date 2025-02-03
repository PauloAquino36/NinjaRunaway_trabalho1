using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Animator animator;
    public float health = 100f;
    private int speed = 8;
    public Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public GameObject kunaiPrefab;
    public Transform KunaiSpawnPointLeft;
    public Transform KunaiSpawnPointRight;
    private int jumpCount = 0;
    private int maxJumps = 3;
    public float glideGravityScale = 0.25f;
    public float originalGravityScale;
    public GameObject attackHitbox; // Referência para a hitbox do ataque

    public bool coowldown = false;
    CapsuleCollider2D colliderOriginal;
    Vector2 colliderSize;
    Vector2 colliderOffset;
    Vector2 colliderSlideSize = new Vector2(3.790423f, 3.790423f);
    Vector2 colliderSlideOffset = new Vector2(0.7352118f, 0.4f);

    private Vector3 startPosition; // Guarda a posição inicial do jogador

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalGravityScale = rb.gravityScale;

        colliderOriginal = GetComponent<CapsuleCollider2D>();

        colliderSize = colliderOriginal.size;
        colliderOffset = colliderOriginal.offset;

        startPosition = transform.position; // Armazena a posição inicial do jogador

        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false); // Desativa a hitbox no início
        }
    }

    void Update()
    {
        move();
        attack();
        corrigeRotate();
        CheckFallDeath(); // Verifica se o jogador caiu do mapa
    }

    public void CheckFallDeath()
    {
        if (transform.position.y < -10) // Caso o jogador caia do mapa
        {
            if (health > 35)
            {
                health -= 35;
                Respawn(); // Teletransporta o jogador de volta
            }
            else
            {
                KillPlayer();
            }
        }
    }

    public void Respawn()
    {
        rb.linearVelocity = Vector2.zero; // Zera a velocidade para evitar quedas contínuas
        transform.position = startPosition; // Teletransporta o jogador para o início
    }

    public void KillPlayer()
    {
        health = 0;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;
        this.enabled = false; // Desativa os controles do jogador
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
            animator.SetBool("isGround", true);
            animator.SetBool("climbMove", false);
        }
    }

    private void move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

    spriteRenderer.flipX = moveInput < 0;
    animator.SetBool("run", moveInput != 0);

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            if (jumpCount < (maxJumps - 1))
            {
                animator.SetTrigger("jump");
                animator.SetBool("isGround", false);
                rb.AddForce(new Vector2(0, jumpCount == 0 ? 5 : 3), ForceMode2D.Impulse);
            }
            jumpCount++;
        }

        if (Input.GetKey(KeyCode.Space) && jumpCount >= maxJumps)
        {
            rb.gravityScale = glideGravityScale;
            animator.SetBool("glider", true);
        }
        else
        {
            rb.gravityScale = originalGravityScale;
            animator.SetBool("glider", false);
        }

        if ((Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) && animator.GetBool("isGround") == true)
        {
            transform.Translate(Input.GetAxis("Horizontal") * speed * (spriteRenderer.flipX ? -6 : 6) * Time.deltaTime, 0, 0);
            animator.SetTrigger("slide");
        }

        if (animator.GetBool("climb") == true && Input.GetKey(KeyCode.W))
        {
            animator.SetBool("climbMove", true);
            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(0, 0);
            transform.Translate(0, 1 * speed * Time.deltaTime, 0);
        }
    }

    private void attack()
    {
        if (!animator.GetBool("run"))
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (coowldown == false && animator.GetBool("glider") == false && animator.GetBool("climbMove") == false)
                {
                    animator.SetTrigger("attack");
                    StartCoroutine(EnableAttackHitbox());
                }
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (coowldown == false && animator.GetBool("glider") == false && animator.GetBool("climbMove") == false)
                {
                    animator.SetTrigger("throw");
                    StartCoroutine(ThrowKunaiWithDelay(0.25f)); // Chama a corrotina com atraso de 0.25s
                }
            }
        }
    }

    private IEnumerator EnableAttackHitbox()
    {
        attackHitbox.SetActive(true); // Ativa a hitbox do ataque
        yield return new WaitForSeconds(0.2f); // Mantém ativa por 0.2s
        attackHitbox.SetActive(false); // Desativa a hitbox após o tempo
    }

    private IEnumerator ThrowKunaiWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Aguarda o tempo especificado

        if (!spriteRenderer.flipX)
            Instantiate(kunaiPrefab, KunaiSpawnPointRight.position, KunaiSpawnPointRight.rotation);
        else
            Instantiate(kunaiPrefab, KunaiSpawnPointLeft.position, KunaiSpawnPointLeft.rotation);
    }

    public void originalCollider()
    {
        colliderOriginal.size = colliderSize;
        colliderOriginal.offset = colliderOffset;
    }

    public void slideCollider()
    {
        colliderOriginal.size = colliderSlideSize;
        colliderOriginal.offset = colliderSlideOffset;
    }

    public void corrigeRotate()
    {
        if (transform.rotation.z != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (transform.rotation.y != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (transform.rotation.x != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void setCooldown()
    {
        coowldown = true;
    }

    public void resetCooldown()
    {
        coowldown = false;
    }
}
