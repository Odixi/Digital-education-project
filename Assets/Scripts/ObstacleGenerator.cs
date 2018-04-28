using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour {

	public GameObject obstaclePrefab;
	public float maxSize;
	public float minSize;
	public float playerMinDistance;

	// Use this for initialization
	void Start () {

		Vector3 pos;
		Vector3 scale;

		var lowerLeft = Camera.main.ScreenToWorldPoint (Vector3.zero);
		var upperRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));

		Player[] players = FindObjectsOfType<Player> ();
		GameObject obstacle;
		for (int i = 0; i < 3; i++) {
		CONTINUE:
			pos = new Vector3 (Random.Range (lowerLeft.x, upperRight.x), Random.Range (lowerLeft.y, upperRight.y),0);
			scale = new Vector3 (Random.Range (minSize, maxSize), Random.Range (minSize, maxSize),1);
			foreach (Player p in players){
				if ((p.transform.position - pos).magnitude - Mathf.Max (scale.x, scale.y) < playerMinDistance) {
					i++;
					goto CONTINUE;
				}
			}
			obstacle = Instantiate (obstaclePrefab);
			obstacle.transform.position = pos;
			obstacle.transform.localScale = scale;
		}

	}
	

}
