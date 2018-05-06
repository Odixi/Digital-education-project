using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour {

	public GameObject obstaclePrefab;
	private float maxSize = 5;
	private float minSize = 3;
	public float playerMinDistance;

	// Use this for initialization
	void Start () {

		Vector3 pos;
		Vector3 scale;

		var lowerLeft = Camera.main.ScreenToWorldPoint (Vector3.zero);
		var upperRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));

		Player[] players = FindObjectsOfType<Player> ();
		GameObject obstacle;
		GameObject obstacle2;

		// Lower obstacle
		pos = new Vector3 ((lowerLeft.x + upperRight.x)/2f + Random.Range(-1.5f, 1.5f),(lowerLeft.y + upperRight.y)/2 + Random.Range(-2f,-5f),0);
		scale = new Vector3 (Random.Range (minSize, maxSize), Random.Range (minSize, maxSize), 1);
		obstacle = Instantiate (obstaclePrefab);
		obstacle.transform.position = pos;
		obstacle.transform.localScale = scale;

		// Upper obstacle(s)
		pos = new Vector3 ((lowerLeft.x + upperRight.x)/2f + Random.Range(-4.5f, 4.5f),(lowerLeft.y + upperRight.y)/2 + Random.Range(2f,5f),0);
		scale = new Vector3 (Random.Range (minSize, maxSize), Random.Range (minSize, maxSize), 1);
		obstacle2 = Instantiate (obstaclePrefab);
		obstacle2.transform.position = pos;
		obstacle2.transform.localScale = scale;
		//print ((obstacle.transform.position.y + obstacle.transform.localScale.y / 2f) - (obstacle2.transform.position.y - obstacle2.transform.localScale.y / 2f));
		UUDESTAAN:
		if ((obstacle.transform.position.y + obstacle.transform.localScale.y / 2f) - (obstacle2.transform.position.y - obstacle2.transform.localScale.y / 2f) > -2.2f) {
			obstacle2.transform.position += new Vector3(0,0.25f,0);
			goto UUDESTAAN;
		}


		/* Old random obstacles 
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
		*/

	}
	

}
