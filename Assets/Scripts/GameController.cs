using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public Text velocityField;
	public Text angleField;
	public GameObject ball;
	private BallMover ballMover;
	public Button throwBtn;


	// Use this for initialization
	void Start () {
		throwBtn.onClick.AddListener (onThrowClicked);
		ballMover = ball.GetComponent<BallMover> ();
	}
		
	void onThrowClicked(){
		ballMover.ResetPosition ();
		ballMover.ThrowBall (float.Parse (velocityField.text), float.Parse (angleField.text));
		print ("throw");
	}

}
