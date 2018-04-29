using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasuringController : MonoBehaviour {

	public Camera camera;
	public GameObject measureText;
	public GameObject measureTextX;
	public GameObject measureTextY;

	private LineRenderer lineRenderer, lineXY;
	public Gradient lineXYGradient;
	private Vector3 pos1,pos2,pos3;

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.positionCount = 2;
		lineRenderer.enabled = false;

		lineXY = (Instantiate (new GameObject ("lineXY"), transform).AddComponent<LineRenderer>());
		lineXY.positionCount = 3;
		lineXY.widthMultiplier = 0.02f;
		lineXY.colorGradient = lineXYGradient;
		lineXY.enabled = false;
		pos3 = Vector3.zero;

	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown (1)) {
			lineRenderer.enabled = true;
			lineXY.enabled = true; // TODO Only show if angle is big enough and pos1 and pos2 distance is big enough
			measureText.GetComponent<Text> ().enabled = true;
			pos1 = camera.ScreenToWorldPoint (Input.mousePosition);
			pos1.z = -2;
			lineRenderer.SetPosition (0, pos1);
			lineXY.SetPosition (0, pos1);
		}

		if (Input.GetMouseButton (1)) {
			pos2 = camera.ScreenToWorldPoint (Input.mousePosition);
			pos2.z = -2;
			pos3.Set (pos2.x, pos1.y, -2);
			lineRenderer.SetPosition (1, pos2);
			lineXY.SetPosition (1, pos3);
			lineXY.SetPosition (2, pos2);

			measureText.GetComponent<Text> ().text = (pos1 - pos2).magnitude.ToString ("0.00") + "m";
			measureText.transform.position = (pos1 + pos2)/2;
		}

		if (Input.GetMouseButtonUp (1)) {
			lineRenderer.enabled = false;
			lineXY.enabled = false;
			measureText.GetComponent<Text> ().enabled = false;
		}

		if (Input.GetKey(KeyCode.R)) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
