using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerSP : MonoBehaviour {

	private Player player;
	public GameObject playerPrefab;
	public GameObject target;
	private GameObject targetInstance;
	public GameObject endText;
	public GameObject endView;

	public Button btnThrow;
	public AnswerField inputs;
	// Timer?

	public GameObject ballPrefab;

	public GameObject triesText;
	private int tries;

	void Start () {
		// TODO instruction thingy
		var ground = FindObjectOfType<GroundGenerator>();
		player = CreatePlayerInstance(1, new Vector2(Camera.main.ScreenToWorldPoint(Vector3.zero).x + Random.Range(1,3), ground.GetLeftHeight()+0.6f), 1);
		targetInstance = Instantiate (target, new Vector2 (Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, 0, 0)).x - Random.Range (1, 3), ground.GetRightHeight () + 0.6f), new Quaternion ());
		GetComponent<ObstacleGenerator> ().enabled = true;
		tries = 0;
	}


	public void OnThrowClicked(){
		player.answerStatus = true;
		player.answerVelocity = float.Parse(inputs.velocityField.text);
		player.answerAngle = float.Parse(inputs.angleField.text);
		player.Shoot (ballPrefab);
		inputs.answerBtn.interactable = false;
	}

	public void Result(bool hitOrMiss){
		if (hitOrMiss) {
			tries++;
			GameWon ();
		} else {
			tries++;
			triesText.GetComponent<Text> ().text = "Tries: " + tries.ToString ();
			inputs.answerBtn.interactable = true;
		}
	}

	private void GameWon(){
		endView.SetActive (true);
		endText.GetComponent<Text> ().text = tries == 1 ? "You won in one try! Well done!" : "You won in " + tries.ToString () + " tries!";
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

	public void RestartGame()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public void EndGame()
	{
		Application.LoadLevel("Menu");
	}


}
