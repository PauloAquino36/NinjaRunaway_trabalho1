using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GameObject attackHitbox;

    public bool coowldown = false;
    CapsuleCollider2D colliderOriginal;
    Vector2 colliderSize;
    Vector2 colliderOffset;
    Vector2 colliderSlideSize = new Vector2(3.790423f, 3.790423f);
    Vector2 colliderSlideOffset = new Vector2(0.7352118f, 0.4f);

    public AudioSource audioSource;
    public AudioClip slashSound;
    public AudioClip kunaiSound;

    public int vidas = 2;

    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalGravityScale = rb.gravityScale;

        colliderOriginal = GetComponent<CapsuleCollider2D>();

        colliderSize = colliderOriginal.size;
        colliderOffset = colliderOriginal.offset;

        startPosition = transform.position;
        audioSource = GetComponent<AudioSource>();

        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }

    void Update()
    {
        move();
        attack();
        corrigeRotate();
        CheckFallDeath();

        if (transform.position.y < -5)
        {
            vidas--;

            if (vidas >= 0)
            {
                transform.position = startPosition;
            }
            else
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    public void CheckFallDeath()
    {
        if (transform.position.y < -10)
        {
            if (health > 35)
            {
                health -= 35;
                Respawn();
            }
            else
            {
                KillPlayer();
            }
        }
    }

    public void Respawn()
    {
        rb.linearVelocity = Vector2.zero; // Corrigido de velocity para linearVelocity
        transform.position = startPosition;
    }

    public void KillPlayer()
    {
        health = 0;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;
        this.enabled = false;
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
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y); // Corrigido de velocity para linearVelocity

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

        if ((Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) && animator.GetBool("isGround"))
        {
            transform.Translate(Input.GetAxis("Horizontal") * speed * (spriteRenderer.flipX ? -6 : 6) * Time.deltaTime, 0, 0);
            animator.SetTrigger("slide");
        }

        if (animator.GetBool("climb") && Input.GetKey(KeyCode.W))
        {
            animator.SetBool("climbMove", true);
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero; // Corrigido de velocity para linearVelocity
            transform.Translate(0, 1 * speed * Time.deltaTime, 0);
        }
    }

    private void attack()
    {
        if (!animator.GetBool("run"))
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (!coowldown && !animator.GetBool("glider") && !animator.GetBool("climbMove"))
                {
                    animator.SetTrigger("attack");
                    StartCoroutine(EnableAttackHitbox());
                    audioSource.PlayOneShot(slashSound);
                }
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (!coowldown && !animator.GetBool("glider") && !animator.GetBool("climbMove"))
                {
                    animator.SetTrigger("throw");
                    StartCoroutine(ThrowKunaiWithDelay(0.25f));
                }
            }
        }
    }

    private IEnumerator EnableAttackHitbox()
    {
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        attackHitbox.SetActive(false);
    }

    private IEnumerator ThrowKunaiWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(kunaiSound);
        Instantiate(kunaiPrefab, spriteRenderer.flipX ? KunaiSpawnPointLeft.position : KunaiSpawnPointRight.position, Quaternion.identity);
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
        if (transform.rotation != Quaternion.identity)
        {
            transform.rotation = Quaternion.identity;
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
