﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
        // Pemain 1
    public PlayerControl player1; // skrip
    private Rigidbody2D player1Rigidbody;
 
    // Pemain 2
    public PlayerControl player2; // skrip
    private Rigidbody2D player2Rigidbody;
 
    // Bola
    public BallControl ball; // skrip
    private Rigidbody2D ballRigidbody;
    private CircleCollider2D ballCollider;
    // Skor maksimal
    public int maxScore;
    // Apakah debug window ditampilkan?
    private bool isDebugWindowShown = false;
    // Objek untuk menggambar prediksi lintasan bola
    public Trajectory trajectory;
    private void Start()
    {
        player1Rigidbody = player1.GetComponent<Rigidbody2D>();
        player2Rigidbody = player2.GetComponent<Rigidbody2D>();
        ballRigidbody = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI() {
        // Kiri atas
        ViewLabel(Screen.width / 2 - 150 - 12, 20, 100, 100, "" + player1.Score);
        // Kanan atas
        ViewLabel(Screen.width / 2 + 150 + 12, 20, 100, 100, "" + player2.Score);

        // Restart
        if (GUI.Button(new Rect(Screen.width / 2 - 60, 35, 120, 53), "RESTART")) {
            player1.ResetScore();
            player2.ResetScore();

            SendMessageToBall("RestartGame", 0.5f);
        }

        // player1 win
        if (player1.Score == maxScore) {
            ViewLabel(Screen.width / 2 - 150, Screen.width / 2 - 10, 20000, 1000, "PLAYER ONE WINS");

            SendMessageToBall("ResetBall", null);
        } else if (player2.Score == maxScore) {
            ViewLabel(Screen.width / 2 + 30, Screen.height / 2 - 10, 2000, 1000, "PLAYER TWO WINS");

            SendMessageToBall("ResetBall", null);
        }

        // Jika isDebugWindowShown == true, tampilkan text area untuk debug window.
        if (isDebugWindowShown) {
            // Save prev color GUI
            Color oldColor = GUI.backgroundColor;
            // new color
            GUI.backgroundColor = Color.red;

            // Save variable physics
            float ballMass = ballRigidbody.mass;
            Vector2 ballVelocity = ballRigidbody.velocity;
            float ballSpeed = ballRigidbody.velocity.magnitude;
            Vector2 ballMomentum = ballMass * ballVelocity; 
            float ballFriction = ballCollider.friction;
 
            float impulsePlayer1X = player1.LastContactPoint.normalImpulse;
            float impulsePlayer1Y = player1.LastContactPoint.tangentImpulse;
            float impulsePlayer2X = player2.LastContactPoint.normalImpulse;
            float impulsePlayer2Y = player2.LastContactPoint.tangentImpulse;
 
            // Tentukan debug text-nya
            string debugText = 
                "Ball mass = " + ballMass+ "\n" +
                "Ball velocity = " + ballVelocity + "\n" +
                "Ball speed = " + ballSpeed + "\n" +
                "Ball momentum = " + ballMomentum + "\n" +
                "Ball friction = " + ballFriction + "\n" +
                "Last impulse from player 1 = (" + impulsePlayer1X + ", " + impulsePlayer1Y + ")\n" +
                "Last impulse from player 2 = (" + impulsePlayer2X + ", " + impulsePlayer2Y + ")\n";

            // Tampilkan debug window
            GUIStyle guiStyle = new GUIStyle(GUI.skin.textArea);
            guiStyle.alignment = TextAnchor.UpperCenter;
            GUI.TextArea(new Rect(Screen.width/2 - 200, Screen.height - 200, 400, 110), debugText, guiStyle);

            // Kembalikan warna lama GUI
            GUI.backgroundColor = oldColor;
        }

        // Toggle nilai debug window ketika pemain mengeklik tombol ini.
        if (GUI.Button(new Rect(Screen.width/2 - 60, Screen.height - 73, 120, 53), "TOGGLE\nDEBUG INFO"))
        {
            isDebugWindowShown = !isDebugWindowShown;
        }
        trajectory.enabled = !trajectory.enabled;
    }

    void SendMessageToBall(string method, object value) {
        ball.SendMessage(method, value, SendMessageOptions.RequireReceiver);
    }

    void ViewLabel(float x, float y, float width, float height, string text) {
        GUI.Label(new Rect(x, y, width, height), text);
    }
}