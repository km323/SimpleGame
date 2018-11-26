using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]
    private Transform front;

    [SerializeField]
    private float moveDistance;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private CapsuleCollider2D capsuleCollider;

    [SerializeField]
    CircleCollider2D collider;

    private const float angleOffset = 90;

    private Vector3 frontDirection;
    private Vector3 targetPosition;
    private bool canInput;
    // Use this for initialization
    void Start () {
        canInput = true;
        capsuleCollider.enabled = false;
        targetPosition = transform.position;
        CalcFrontDirection();
    }
	
	// Update is called once per frame
	void Update () {
        InputHandle();
        CalcFrontDirection();
    }

    private void FixedUpdate()
    {
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
    }

    void CalcFrontDirection()
    {
        frontDirection = (front.position - transform.position).normalized;
        Rotation(frontDirection);
    }
    void CalcTargetPosition()
    {
        Vector2 direction = frontDirection.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x);
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);

        //targetPosition = transform.position + new Vector3(x, y, 0) * moveDistance;
        transform.position += new Vector3(x, y, 0) * moveDistance;
    }
    void InputHandle()
    {
        if (!canInput)
            return;

        capsuleCollider.enabled = false;

        Vector2 input;
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        if (input.magnitude <= 0.1f)
            return;
        Rotation(input);

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            capsuleCollider.enabled = true;
            CalcTargetPosition();
        }
        transform.position += new Vector3(input.x * moveSpeed * Time.fixedDeltaTime, input.y * moveSpeed * Time.fixedDeltaTime,0);
    }

    void Rotation(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetAngle = Quaternion.Euler(new Vector3(0, 0, angle - angleOffset));
        transform.rotation = targetAngle;
    }

    IEnumerator Dead()
    {
        canInput = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;

        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();

        yield return new WaitForSeconds(particleSystem.main.duration);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            StartCoroutine(Dead());
            Destroy(collision.gameObject);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Background")
        {
            targetPosition = collision.contacts[0].normal * collider.radius;
            targetPosition += transform.position;
        }
    }
}
