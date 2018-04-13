using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public bool answerStatus;
    public float answerVelocity;
    public float answerAngle;
    public float answerTime;
    public bool shotLeft;

	// Use this for initialization
	public Player() {
        answerStatus = false;
        answerVelocity = 0f;
        answerAngle = 0f;
        shotLeft = false;
        answerTime = 0f;
	}
	
    public void Shoot(BallMover mover)
    {
        mover.ResetPosition();
        mover.ThrowBall(answerVelocity, answerAngle);
        shotLeft = true;
    }

	
}
