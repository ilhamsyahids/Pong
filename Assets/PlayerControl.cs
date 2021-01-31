using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Tombol untuk menggerakkan ke atas
    public KeyCode upButton = KeyCode.W;
 
    // Tombol untuk menggerakkan ke bawah
    public KeyCode downButton = KeyCode.S;
 
    // Kecepatan gerak
    public float speed = 10.0f;
 
    // Batas atas dan bawah game scene (Batas bawah menggunakan minus (-))
    public float yBoundary = 9.0f;
 
    // Rigidbody 2D raket ini
    private Rigidbody2D rigidBody2D;
 
    // Skor pemain
    private int score;
    // Titik tumbukan terakhir with ball
    private ContactPoint2D lastContactPoint;
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // move player
        Vector2 velocity = rigidBody2D.velocity;

        if (Input.GetKey(upButton)) { // ke atas
            velocity.y = speed;
        } else if (Input.GetKey(downButton)) { // ke bawah
            velocity.y = -speed;
        } else {
            velocity.y = 0.0f;
        }

        rigidBody2D.velocity = velocity;


        // boundary
        Vector3 position = transform.position;

        if (position.y > yBoundary) {
            position.y = yBoundary;
        } else if (position.y < -yBoundary) {
            position.y = -yBoundary;
        }

        transform.position = position;
    }

    public void IncrementScore() {
        score++;
    }

    public void ResetScore() {
        score = 0;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.name.Equals("Ball")) {
            lastContactPoint = other.GetContact(0);
        }
    }

    public int Score {
        get {
            return score;
        }
    }

    public ContactPoint2D LastContactPoint {
        get {
            return lastContactPoint;
        }
    }
}
