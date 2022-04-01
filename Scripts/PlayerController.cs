using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform cam;

    public float speed = 10f;

    public float turnSmoothTime = 0.1f;

    float turnSmoothVelocity;

    float horizontal;

    float vertical;

    public float jumpForce;

    private bool canJump = true;


    Animator ani;

    private Rigidbody rb;
    
    void Start()
    {
        cam = Camera.main.transform;
        ani = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            ani.SetTrigger("jump");
            this.rb.AddForce(Vector3.up * jumpForce);
            canJump = false;
        }
    }

    void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;

        

        if ((dir.magnitude >= 0.1f))
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            this.rb.velocity = this.rb.velocity.y * Vector3.up + moveDir * speed;

        }
        else
        {
            this.rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        

        playAni();
    }

    private void LateUpdate()
    {
        this.transform.position = this.rb.transform.position;
    }

    void playAni()
    {
        ani.SetFloat("horizontal", Mathf.Abs(horizontal));
        ani.SetFloat("vertical", Mathf.Abs(vertical));
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }
}
