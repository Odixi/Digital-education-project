using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMover : MonoBehaviour {

	//public Vector3 startPosition;
	public Vector2 velocity;
	public float g = 9.81f;
	public bool simulate = false;

	// Use this for initialization
	void Start () {
		
	}

	public void ThrowBall(float vel, float ang){

        Debug.Log("NYT");
        velocity = Vector2.right * vel;
        velocity = Quaternion.Euler (0, 0, ang) * velocity;
		simulate = true;
	}

	void FixedUpdate () {
        Debug.Log(transform.position);
        if (simulate) {
			//gravity
			velocity.y = velocity.y - g * Time.fixedDeltaTime;

            

            // moving of the ball
            float x = transform.position.x + velocity.x * Time.fixedDeltaTime;
			float y = transform.position.y + velocity.y * Time.fixedDeltaTime;
			transform.position = new Vector3 (x, y, 0);
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
        //ResetPosition ();
        Debug.Log("TÄSÄ");
        simulate = false;
        GameObject.Destroy(this.gameObject);
    }
    
    public void ResetPosition(Vector3 position){
        transform.position = position;
        //transform.position = startPosition;
    }
}
