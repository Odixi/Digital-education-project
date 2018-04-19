using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasuringController : MonoBehaviour {

	public Camera camera;
	public GameObject measureText;

	private LineRenderer lineRenderer;
	private Vector3 pos1,pos2;

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.positionCount = 2;
		lineRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown (1)) {
			lineRenderer.enabled = true;
			measureText.GetComponent<Text> ().enabled = true;
			pos1 = camera.ScreenToWorldPoint (Input.mousePosition);
			pos1.z = -2;
			lineRenderer.SetPosition (0, pos1);
		}

		if (Input.GetMouseButton (1)) {
			pos2 = camera.ScreenToWorldPoint (Input.mousePosition);
			pos2.z = -2;
			lineRenderer.SetPosition (1, pos2);

			measureText.GetComponent<Text> ().text = (pos1 - pos2).magnitude.ToString ("0.00") + "m";
			measureText.transform.position = (pos1 + pos2)/2;
		}

		if (Input.GetMouseButtonUp (1)) {
			lineRenderer.enabled = false;
			measureText.GetComponent<Text> ().enabled = false;
		}
	}
}
