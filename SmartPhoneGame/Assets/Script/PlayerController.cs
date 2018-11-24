using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TouchScript.Gestures;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private FlickGesture flickGesture;

    [SerializeField]
    CircleCollider2D collider;

    [SerializeField]
    private float moveDistance;

    [SerializeField]
    private float moveSpeed;

    private Vector3 targetPosition;
    private Vector2 flickDirection;

    private void OnEnable()
    {
        flickGesture.Flicked += OnFlicked;
    }

    private void OnDisable()
    {
        flickGesture.Flicked -= OnFlicked;
    }

    private void OnFlicked(object sender, EventArgs e)
    {
        CalculateTargetPosition();
        //Debug.Log("フリックされた: " + flickGesture.ScreenFlickVector);
    }

    private void CalculateTargetPosition()
    {
        flickDirection = flickGesture.ScreenFlickVector.normalized;

        float angle = Mathf.Atan2(flickDirection.y, flickDirection.x);
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);

        targetPosition = transform.position + new Vector3(x, y, 0) * moveDistance;
    }

    void Start()
    {
        targetPosition = transform.position;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
    }

    IEnumerator Dead()
    {
        GetComponent<FlickGesture>().enabled = false;
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
