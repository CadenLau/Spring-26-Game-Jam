using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform arrowTransform;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float accelSpeed = 200f;
    [SerializeField] private float decelSpeed = 200f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashPauseDuration = 0.6f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float arrowDistance = 1.5f;
    [SerializeField] private float aimTimeScale = 0.05f;

    [Header("Gravity")]
    [SerializeField] private float gravityScale = 3f;
    [SerializeField] private float gravityScalingFactor = 2f;

    [Header("Dash Enable")]
    [SerializeField] private string dashZoneTag = "Puddle"; // or "DashZone"

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;

    private Vector2 moveDirection;
    private bool canDash;
    private bool isDashing;
    private bool isChoosingDirection;

    private float originalGravity;
    private RigidbodyConstraints2D originalConstraints;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (rb == null) rb = GetComponent<Rigidbody2D>();

        // Cache actions once (and avoid repeated string lookup)
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        lookAction = playerInput.actions["Look"];

        // Basic sanity
        if (arrowTransform != null)
            arrowTransform.gameObject.SetActive(false);

        rb.gravityScale = gravityScale;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // keep upright
    }

    private void OnEnable()
    {
        jumpAction.performed += Dash;
    }

    private void OnDisable()
    {
        jumpAction.performed -= Dash;
    }

    private void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (isDashing || isChoosingDirection) return;

        float targetSpeed = moveDirection.x * moveSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;

        float accelRate = Mathf.Abs(targetSpeed) > 0.05f ? accelSpeed : decelSpeed;
        float movement = speedDiff * accelRate * Time.fixedDeltaTime;

        rb.AddForce(Vector2.right * movement, ForceMode2D.Force);

        // Gravity scaling (faster fall)
        rb.gravityScale = (rb.linearVelocity.y < 0) ? gravityScale * gravityScalingFactor : gravityScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(dashZoneTag))
            canDash = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(dashZoneTag))
            canDash = false;
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (!canDash || isChoosingDirection || arrowTransform == null) return;
        StartCoroutine(DashCoroutine());
    }

    private System.Collections.IEnumerator DashCoroutine()
    {
        isChoosingDirection = true;

        // Freeze motion without disabling simulation
        originalGravity = rb.gravityScale;
        originalConstraints = rb.constraints;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        // Slow time for aiming (use unscaled time in loop)
        float prevTimeScale = Time.timeScale;
        Time.timeScale = aimTimeScale;

        arrowTransform.gameObject.SetActive(true);

        Vector2 direction = Vector2.right;
        float timer = 0f;

        while (timer < dashPauseDuration)
        {
            Vector2 pointerScreenPos = lookAction.ReadValue<Vector2>();
            Vector2 pointerWorldPos = Camera.main.ScreenToWorldPoint(pointerScreenPos);

            direction = (pointerWorldPos - (Vector2)transform.position).normalized;

            // rotate arrow to point towards pointer
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrowTransform.rotation = Quaternion.Euler(0, 0, angle);
            // position arrow at fixed distance in that direction
            arrowTransform.position = transform.position + (Vector3)(direction * arrowDistance);

            // Press again to dash early
            if (jumpAction.WasPressedThisFrame() && timer > 0.1f)
                break;

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        arrowTransform.gameObject.SetActive(false);

        // Restore time
        Time.timeScale = prevTimeScale;

        // Dash
        isChoosingDirection = false;
        isDashing = true;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = originalGravity;
        rb.linearVelocity = direction * dashSpeed;

        yield return new WaitForSecondsRealtime(dashDuration);

        isDashing = false;
    }
}