using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    private Vector2 moveDirection;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float accelSpeed;
    [SerializeField] private float decelSpeed;
    [SerializeField] private float dashSpeed;

    private Vector2 dashDirection;
    private bool canDash = false;
    private bool isDashing;


    private PlayerInput playerInput;

    [SerializeField] private float gravityScale;
    [SerializeField] private float gravityScalingFactor;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.actions["Jump"].performed += Dash;
    }

    private void Update()
    {
        moveDirection = playerInput.actions["Move"].ReadValue<Vector2>();
    }

    private void Start()
    {
        rb.gravityScale = gravityScale;
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canDash = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        canDash = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canDash = false;
    }

    private void OnDisable()
    {
        playerInput.actions["Jump"].performed -= Dash;
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (!canDash) return;

        Vector2 mouseScreenPos = playerInput.actions["Look"].ReadValue<Vector2>();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;

        isDashing = true;

        rb.linearVelocity = direction * dashSpeed;

        Invoke(nameof(EndDash), 0.2f);
    }

    private void EndDash()
    {
        isDashing = false;
    }
}
