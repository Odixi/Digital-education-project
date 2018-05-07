using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasuringController : MonoBehaviour {

	public Camera camera;
	public GameObject measureText;
	public GameObject measureTextX;
	public GameObject measureTextY;
	public GameObject measureTextAngle;

	private LineRenderer lineRenderer, lineXY;
	public Gradient lineXYGradient;
	public Material lineXYMaterial;
	private Vector3 pos1,pos2,pos3;

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.positionCount = 2;
		lineRenderer.enabled = false;

		lineXY = (Instantiate (new GameObject ("lineXY"), transform).AddComponent<LineRenderer>());
		lineXY.positionCount = 3;
		lineXY.widthMultiplier = 0.02f;
		lineXY.material = lineXYMaterial;
		lineXY.colorGradient = lineXYGradient;
		lineXY.enabled = false;
		pos3 = Vector3.zero;

	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown (1)) {
			lineRenderer.enabled = true;
			lineXY.enabled = true;
			measureText.GetComponent<Text> ().enabled = true;
			measureTextAngle.GetComponent<Text> ().enabled = true;
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
			measureText.transform.position = (pos1 + pos2) / 2;

			// Let's hide the other values if they are too close together
			if ((pos1 - pos2).magnitude > 2 ) {

				if ((measureText.transform.position - measureTextX.transform.position).magnitude > 0.3f)
					measureTextX.GetComponent<Text> ().enabled = true;
				else
					measureTextX.GetComponent<Text> ().enabled = false;
				
				if ((measureText.transform.position - measureTextY.transform.position).magnitude > 0.6f)
					measureTextY.GetComponent<Text> ().enabled = true;
				else
					measureTextY.GetComponent<Text> ().enabled = false;

				measureTextX.GetComponent<Text> ().text = (pos1 - pos3).magnitude.ToString ("0.00") + "m";
				measureTextX.transform.position = (pos1 + pos3) / 2;

				measureTextY.GetComponent<Text> ().text = (pos2 - pos3).magnitude.ToString ("0.00") + "m";
				measureTextY.transform.position = (pos2 + pos3) / 2;

			} else {
				measureTextX.GetComponent<Text> ().enabled = false;
				measureTextY.GetComponent<Text> ().enabled = false;
			}

			measureTextAngle.GetComponent<Text> ().text = Vector3.Angle ((pos1 - pos2), (pos1 - pos3)).ToString ("0.00") + "\u00B0";
			//measureTextAngle.transform.position = (pos1 + (pos1-pos2).normalized*0.4f);
			measureTextAngle.transform.position = (pos1 + ((pos1-pos2 + pos1-pos3)/2f).normalized*0.4f);
		
		}

		if (Input.GetMouseButtonUp (1)) {
			lineRenderer.enabled = false;
			lineXY.enabled = false;
			measureText.GetComponent<Text> ().enabled = false;
			measureTextX.GetComponent<Text> ().enabled = false;
			measureTextY.GetComponent<Text> ().enabled = false;
			measureTextAngle.GetComponent<Text> ().enabled = false;
		}

		if (Input.GetKey(KeyCode.R)) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
