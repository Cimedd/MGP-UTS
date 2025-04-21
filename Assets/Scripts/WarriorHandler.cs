using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using static UnityEngine.UI.Image;

public class WarriorHandler : MonoBehaviour
{
    public int health = 5;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 destination;
    private Quaternion lookRotation, oldRotation;
    private float currentMotion = 0.0f;
    private float motionSpeed = 1.0f;
    private float rotationSpeed = 5.0f;
    private float orbitX = 100f;
    private float orbitY = 10f;
    private Vector2 delta;
    private int countShoot = 0;
    private bool isMove = false;
    public int ammo = 30;

    public bool isDashing = false;
    public float dashingRange = 5f;
    public float dashTime = 0.5f;
    public float dashCooldown = 2f;
    public CinemachineFreeLook freeLook;

    public bool isShooting = false;
    public Transform shootPoint;
    public Vector3 origin, direction;
    public float damage = 5;

    public UIManager uimanager;

    [SerializeField] private float moveSpeed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponent<Animator>();
       rb=GetComponent<Rigidbody>();

        dashCooldown = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(dashCooldown > 0f)
        {
            dashCooldown -= 1 * Time.deltaTime;
        }
       
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Dash());
        }
        //TouchScreem
        /* if (Touchscreen.current.primaryTouch.press.isPressed)
         {
             transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
             transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
             currentMotion += (motionSpeed * Time.deltaTime);
             if(currentMotion > 1.0f)
             {
                 currentMotion = 1.0f;
             }
             animator.SetFloat("Motion", currentMotion);
         }
         else
         {
             currentMotion = 0.0f;
             animator.SetFloat("Motion", currentMotion);
             if (currentMotion < 0.0f)
             {
                 currentMotion = 0.0f;
             }
             animator.SetFloat("Motion", currentMotion);
         }
 */

        //Controller
        if (destination.magnitude > 0.0f)
        {
            Vector3 lookDirection = new Vector3(destination.x, 0.0f, destination.y);
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(lookRotation * oldRotation, transform.rotation, rotationSpeed * Time.deltaTime);
            

            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            animator.SetFloat("Motion", destination.magnitude);
        }
        else
        {
            float temp = animator.GetFloat("Motion");
            isMove= false;
            animator.SetFloat("Motion", temp - (2.0f * Time.deltaTime));
        }
        freeLook.m_XAxis.Value += delta.x * orbitX * Time.deltaTime;
        freeLook.m_YAxis.Value += delta.y * orbitY   * Time.deltaTime;

       
    }

    public void OnLookAround(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.ReadValue<Vector2>().ToString());
        delta = ctx.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext ctx)
    {
        if(ammo > 0)
        {
            countShoot++;
            Debug.Log("Shoot" + countShoot);
            ammo -= 1;
            animator.SetTrigger("Shoot");
            if (Physics.Raycast(origin, direction, out RaycastHit hit, 13f))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    enemy.TakeDamage(damage);
                    Debug.Log("Enemy hit: " + hit.collider.name);
                }
                else
                {
                    Debug.Log("Hit something else: " + hit.collider.name);
                }
            }
            uimanager.updateAmmo(ammo);
        }
     
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        /* Debug.Log("Jump");
         animator.SetTrigger("Jump");*/
     
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.ReadValue<Vector2>().ToString());
        destination = ctx.ReadValue<Vector2>();

        if(!isMove)
        {
            isMove = true;
            oldRotation = transform.rotation;
        }

    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (dashCooldown <= 0)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        Debug.Log("Dash");
        isDashing = true;
        Vector3 dashDirection = destination == Vector3.zero
                ? transform.forward
                 : new Vector3(destination.x, 0, destination.y).normalized;
        rb.velocity = dashDirection * dashingRange; 
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        rb.velocity = Vector3.zero;
        dashCooldown = 2f;
    }

    private void OnDrawGizmos()
    {
        origin = shootPoint.position;
        direction = shootPoint.forward;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + direction * 10f);
    }


    public void TakeDamage()
    {
        health -= 1;
    }
    /* public void OnRun(InputAction.CallbackContext ctx)
     {
         var touch = Touchscreen.current.primaryTouch;
         Debug.Log("Position : " + touch.position.ReadValue());

         Ray originRay = Camera.main.ScreenPointToRay(
             new Vector3(touch.position.x.ReadValue(), touch.position.y.ReadValue(), 0.0f));
         RaycastHit hitInfo;
         if(Physics.Raycast(originRay, out hitInfo))
         {
             if (hitInfo.transform.tag != "Player")
             {
                 destination = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                 lookRotation = Quaternion.LookRotation(destination, Vector3.up);
                 currentMotion = 1.0f;
                 animator.SetFloat("Motion", currentMotion);
             }
         }
     }*/
}
