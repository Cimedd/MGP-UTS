using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class WarriorHandler : MonoBehaviour
{
    public int health = 5;
    private Animator animator;
    private Rigidbody rb;

    private Vector2 moveInput;
    private Vector2 lookInput;

    public float moveSpeed = 10f;
    public float rotationSpeed = 5f;

    public CinemachineFreeLook freeLook;

    public int ammo = 30;
    private int countShoot = 0;
    public Transform shootPoint;
    public float damage = 5f;

    public UIManager uimanager;

    [Header("Dash Settings")]
    public bool isDashing = false;
    public float dashingRange = 5f;
    public float dashTime = 0.5f;
    private float dashCooldownTimer = 0f;
    public float dashCooldown = 2f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        uimanager.updateAmmo(ammo);
        uimanager.updateHealth(health);
    }

    private void Update()
    {
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;

        HandleMovement();
        HandleCamera();
    }

    private void HandleMovement()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
        if (moveDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            animator.SetFloat("Motion", 1f);
        }
        else
        {
            animator.SetFloat("Motion", 0f);
        }
    }

    private void HandleCamera()
    {
        freeLook.m_XAxis.Value += lookInput.x * 100f * Time.deltaTime;
        freeLook.m_YAxis.Value += lookInput.y * 10f * Time.deltaTime;
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnLookAround(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            HandleShoot();
    }

    // NEW: This allows UI Button to call shooting logic
    public void ShootFromButton()
    {
        HandleShoot();
    }

    private void HandleShoot()
    {
        if (ammo <= 0) return;

        ammo--;
        countShoot++;
        uimanager.updateAmmo(ammo);
        animator.SetTrigger("Shoot");

        Vector3 origin = shootPoint.position;
        Vector3 direction = shootPoint.forward;

        Debug.DrawRay(origin, direction * 50f, Color.red, 2f);
        if (Physics.Raycast(origin, direction, out RaycastHit hit, 50f))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Debug.Log("Hit enemy: " + hit.collider.name);
                }
            }
            else
            {
                Debug.Log("Hit: " + hit.collider.name);
            }
        }
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && dashCooldownTimer <= 0)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        Vector3 dashDir = new Vector3(moveInput.x, 0f, moveInput.y);
        if (dashDir == Vector3.zero)
            dashDir = transform.forward;

        rb.velocity = dashDir.normalized * dashingRange;
        yield return new WaitForSeconds(dashTime);

        rb.velocity = Vector3.zero;
        isDashing = false;
        dashCooldownTimer = dashCooldown;
    }

    public void TakeDamage()
    {
        health--;
        uimanager.updateHealth(health);
        Debug.Log("Player got hit!");

        if (health <= 0)
        {
            uimanager.GameOver();
        }
    }

    private void OnDrawGizmos()
    {
        if (shootPoint == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(shootPoint.position, shootPoint.position + shootPoint.forward * 10f);
    }
}
