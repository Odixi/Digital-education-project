﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMover : MonoBehaviour {

	//public Vector3 startPosition;
	public Vector2 velocity;
	public float g = 9.81f;
	public bool simulate = false;
    public int thrower;
    public GameController controller;

	// Use this for initialization
	void Start () {
		
	}

	public void ThrowBall(float vel, float ang, int throwerNum){
        
        velocity = Vector2.right * vel;
        velocity = Quaternion.Euler (0, 0, ang) * velocity;
        thrower = throwerNum;
		simulate = true;
	}

	void FixedUpdate () {
        if (simulate) {
			//gravity
			velocity.y = velocity.y - g * Time.fixedDeltaTime;

            // moving of the ball
            float x = transform.position.x + velocity.x * Time.fixedDeltaTime;
			float y = transform.position.y + velocity.y * Time.fixedDeltaTime;
			transform.position = new Vector3 (x, y, 0);

            // Below line is in case someone wants to test the boundaries of the throw
			if (transform.position.y < -10 || Mathf.Abs(transform.position.x) > 50 || transform.position.y > 100){ 
				if (controller != null) {
					controller.BallDestroyed (this, 0);
				} else {
					GameControllerSP gosp= FindObjectOfType<GameControllerSP> ();
					Destroy (gameObject);
					gosp.Result (false);
				}
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
        simulate = false;

        //1 = enemy | 0 = ground/obstacle/other ball | -1 = self
        int hitEvaluator = 0;                                              


        if (collision.collider.gameObject.tag.Equals("Player"))
        {
            Player hitTarget = collision.collider.gameObject.GetComponent<Player>();
            if (int.Parse(hitTarget.playerNumber.text) == thrower) hitEvaluator = -1;
            else hitEvaluator = 1;
        }

		if (collision.collider.gameObject.tag.Equals ("Target")) {
			GameControllerSP gosp= FindObjectOfType<GameControllerSP> ();
			Destroy (gameObject);
			gosp.Result (true);
			return;
		}

		if (controller != null) {
			controller.BallDestroyed (this, hitEvaluator);
		} else {
			GameControllerSP gosp= FindObjectOfType<GameControllerSP> ();
			Destroy (gameObject);
			gosp.Result (false);
		}
    }
    
    public void ResetPosition(Vector3 position){
        transform.position = position;
    }
}
