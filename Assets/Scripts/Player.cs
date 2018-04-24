using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public BoxCollider2D hitDetectCollider;
    public GameObject throwPoint; // the point, from which the players throw starts
    public Text playerNumber;

    [HideInInspector]
    public bool answerStatus;
    [HideInInspector]
    public float answerVelocity;
    [HideInInspector]
    public float answerAngle;
    [HideInInspector]
    public float answerTime;
    [HideInInspector]
    public bool shotLeft;
    [HideInInspector]
    public int direction;
    [HideInInspector]
    public float position;

	
    public BallMover Shoot(GameObject thrownBall)
    {
        BallMover mover = GameObject.Instantiate(thrownBall).GetComponent<BallMover>();
        mover.ResetPosition(throwPoint.transform.position);
        mover.ThrowBall(answerVelocity * direction, answerAngle * direction, int.Parse(playerNumber.text));
        shotLeft = true;
        return mover;
    }
    

	
}
