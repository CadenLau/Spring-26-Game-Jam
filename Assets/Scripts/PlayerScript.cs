using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    private Vector2 moveDirection;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float accelSpeed;
    [SerializeField] private float decelSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float maxFallSpeed = 20f;

    private Vector2 dashDirection;
    private bool canDash = false;
    private bool isDashing;
    [SerializeField] private float dashPauseDuration = 1f;
    [SerializeField] private float dashDuration = 0.2f;
    private bool isChoosingDirection;
    private float originalGravity;

    [SerializeField] private Transform arrowTransform;
    [SerializeField] private float arrowDistance = 1.5f;

    [SerializeField] private float gravityScale;
    [SerializeField] private float gravityScalingFactor;

    [SerializeField] private float stunTime = 1f;
    private bool isStunned;

    [SerializeField] private float windForce = 5f;
    private bool applyWind;

    [SerializeField] private EndUIScript endUIScript;

    private PlayerInput playerInput;
    public PlayerInput Input => playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.actions["Jump"].performed += Dash;
    }

    private void Start()
    {
        rb.gravityScale = gravityScale;
    }

    private void Update()
    {
        moveDirection = playerInput.actions["Move"].ReadValue<Vector2>();
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;

        Vector3 left = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 right = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));

        pos.x = Mathf.Clamp(pos.x, left.x + transform.localScale.x / 2f, right.x - transform.localScale.x / 2f);

        transform.position = pos;
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocityY < -maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, -maxFallSpeed);
        }
        if (applyWind)
        {
            rb.AddForce(new Vector2(windForce, 0), ForceMode2D.Force);
        }
        if (isDashing || isStunned) return;

        float targetSpeed = moveDirection.x * moveSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;

        float accelRate = Mathf.Abs(targetSpeed) > 0.05f
            ? accelSpeed
            : decelSpeed;

        float movement = speedDiff * accelRate * Time.fixedDeltaTime;
        rb.AddForce(Vector2.right * movement, ForceMode2D.Force);

        // Gravity scaling
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = gravityScale * gravityScalingFactor;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Cloud"))
        {
            // Trigger win
            GetComponent<SpriteRenderer>().enabled = false; // Hide player sprite
            enabled = false; // Disable player control
            rb.linearVelocity = Vector2.zero; // Stop player movement
            rb.gravityScale = 0; // Disable gravity
            endUIScript.ShowEndScreen();
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Raindrop"))
        {
                    canDash = true;
        } 
        else if (collision.CompareTag("Lightning"))
        {
            LightningHit();
        } 
        else if (collision.CompareTag("Wind"))
        {
            applyWind = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Raindrop"))
        {
                    canDash = true;
        } 
        else if (collision.CompareTag("Wind"))
        {
            applyWind = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Raindrop"))
        {
                    canDash = false;
        } 
        else if (collision.CompareTag("Wind"))
        {
            applyWind = false;
        }
    }

    private void OnDisable()
    {
        playerInput.actions["Jump"].performed -= Dash;
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (!canDash || isChoosingDirection || isStunned) return;
        StartCoroutine(DashCoroutine());
    }

    private System.Collections.IEnumerator DashCoroutine()
    {
        isChoosingDirection = true;

        // Freeze physics
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // Slow time (a lot)
        Time.timeScale = 0.03f;

        float timer = 0f;

        arrowTransform.gameObject.SetActive(true);
        Vector2 direction = Vector2.right;

        // Wait for player to choose direction or dash immediately if they press again
        while (timer < dashPauseDuration)
        {
            // Get direction
            Vector2 mouseScreenPos = playerInput.actions["Look"].ReadValue<Vector2>();
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

            direction = (mouseWorldPos - (Vector2)transform.position).normalized;

            // Rotate arrow
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrowTransform.rotation = Quaternion.Euler(0, 0, angle);

            // Position arrow
            arrowTransform.position = transform.position + (Vector3)(direction * arrowDistance);

            // Dash immediately if player presses again
            if (playerInput.actions["Jump"].WasPressedThisFrame() && timer > 0.1f)
                break;

            timer += Time.unscaledDeltaTime;

            yield return null;
        }

        arrowTransform.gameObject.SetActive(false);

        // Resume time
        Time.timeScale = 1f;

        isChoosingDirection = false;
        isDashing = true;

        // Unfreeze physics
        rb.gravityScale = originalGravity;
        rb.linearVelocity = direction * dashSpeed;
        rb.simulated = true;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }

    public void LightningHit()
    {
        isStunned = true;
        moveDirection = Vector2.zero;
        GetComponent<SpriteRenderer>().color = Color.yellow; // Change color to indicate stun
        Invoke(nameof(EndStun), stunTime);
    }

    private void EndStun()
    {
        isStunned = false;
        GetComponent<SpriteRenderer>().color = Color.white; // Revert color
    }
}
