using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 8f;
    public float jumpForce = 12f;
    private Rigidbody2D rb;
    private float moveInput;
    private float originalScaleX;

    [Header("Detección de Suelo")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [Header ("Inventario de Datos")]
    public int redData = 0;
    public int blueData = 0;
    public int greenData = 0;

    [Header ("Salud y Daño")]
    public int health = 3;
    private bool isInvincible = false;
    public float invincibilityDuration = 1.5f;

    [Header ("Sonidos")]
    private AudioSource fuenteAudio;
    public AudioClip sonidoBueno;
    public AudioClip sonidoMalo;
    public AudioClip sonidoDeposito;
    public AudioClip sonidoVictoria;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Guardamos la escala inicial para que no se haga pequeño al girar
        originalScaleX = transform.localScale.x;

        fuenteAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 1. Detección de suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // 2. Salto
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // 3. Movimiento Horizontal
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // 4. Lógica de Giro (Flip)
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(originalScaleX, transform.localScale.y, transform.localScale.z);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-originalScaleX, transform.localScale.y, transform.localScale.z);
        }
    }

    // 5. RECOGIDA DE DATOS (Mediante el Trigger del Recogedor o del propio Player)
    void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("Data")) {
            DataToken token = other.GetComponent<DataToken>();
            if (token != null) {
                if (token.type == DataToken.DataType.Red) redData++;
                else if (token.type == DataToken.DataType.Blue) blueData++;
                else if (token.type == DataToken.DataType.Green) greenData++;
                Destroy(other.gameObject);
                fuenteAudio.PlayOneShot(sonidoBueno);
            }
        }

        if (other.CompareTag("Enemy") && !isInvincible) {
            TakeDamage();
            Destroy(other.gameObject);
            fuenteAudio.PlayOneShot(sonidoMalo);
        }
    }

    public void TakeDamage() {
        health--;
        if (health <= 0) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else StartCoroutine(BecomeInvincible());
    
    }
    // 6. DAÑO POR ENEMIGOS (Colisión física con "Bug")
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health--;
            Debug.Log("¡ALERTA! Bug detectado. Integridad del sistema: " + health);

            if (health <= 0)
            {
                Debug.Log("ERROR FATAL: Sistema caído.");
                // Reinicia el nivel
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        }
    }
    System.Collections.IEnumerator BecomeInvincible(){
        isInvincible = true;
        GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.5f);
        yield return new WaitForSeconds (invincibilityDuration);
        GetComponent<SpriteRenderer>().color = Color.white;
        isInvincible = false;
    }
}