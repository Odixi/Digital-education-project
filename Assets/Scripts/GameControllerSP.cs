using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerSP : MonoBehaviour {

	private Player player;
	public GameObject playerPrefab;

	public Button btnThrow;
	public AnswerField inputs;
	// Timer?

	public GameObject ballPrefab;


	// Use this for initialization
	void Start () {
		// TODO instruction thingy
		var ground = FindObjectOfType<GroundGenerator>();
		player = CreatePlayerInstance(1, new Vector2(Camera.main.ScreenToWorldPoint(Vector3.zero).x + Random.Range(1,3), ground.GetLeftHeight()+0.6f), 1);
		GetComponent<ObstacleGenerator> ().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnThrowClicked(){
		player.answerStatus = true;
		player.answerVelocity = float.Parse(inputs.velocityField.text);
		player.answerAngle = float.Parse(inputs.angleField.text);
		player.Shoot (ballPrefab);
	}

	private Player CreatePlayerInstance(int dir, Vector2 pos, int num){
		Vector3 readyPositions = playerPrefab.transform.position;
		Player createdPlayer = GameObject.Instantiate(playerPrefab, new Vector3(pos.x, pos.y, readyPositions.z), new Quaternion()).GetComponent<Player>();
		createdPlayer.direction = dir; createdPlayer.position = pos.x; createdPlayer.playerNumber.text = num.ToString();
		if (dir < 0)
		{
			Vector3 throwPointInverted = createdPlayer.throwPoint.transform.localPosition;
			createdPlayer.throwPoint.transform.localPosition = new Vector3(throwPointInverted.x*dir, throwPointInverted.y, throwPointInverted.z);
		}
		return createdPlayer;
	}
}
