using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundGenerator : MonoBehaviour {

	// slopa maximum dimensions
	public float heightMax;
	public float heightMin;
	public float widthMax; 
	public float widthMin;
	public Color color;

	private float leftHeight;
	private float rightHeight;

	private void Awake () {

		var lowerLeft = Camera.main.ScreenToWorldPoint (Vector3.zero);
		var upperRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));

		widthMax = upperRight.x / 2f;

		leftHeight = Random.Range (heightMin, heightMax);
		rightHeight = Random.Range (heightMin, heightMax);
		float lowerAmount = Mathf.Min (leftHeight, rightHeight) - heightMin;
		leftHeight -= lowerAmount;
		rightHeight -= lowerAmount;
		float leftWidth = Random.Range (widthMin, widthMax);
		float RightWidth = Random.Range (widthMin, widthMax);

		var vertices2D = new Vector2[]{
			new Vector2(lowerLeft.x, lowerLeft.y),
			new Vector2(lowerLeft.x, lowerLeft.y + leftHeight),
			new Vector2(lowerLeft.x + leftWidth, lowerLeft.y + leftHeight),
			new Vector2(upperRight.x - RightWidth, lowerLeft.y + rightHeight),
			new Vector2(upperRight.x, lowerLeft.y + rightHeight),
			new Vector2(upperRight.x, lowerLeft.y)
		};

		// Just reusing variables
		leftHeight += lowerLeft.y;
		rightHeight += lowerLeft.y;

		var vertices3D = System.Array.ConvertAll<Vector2, Vector3>(vertices2D, v => v);

		// Use the triangulator to get indices for creating triangles
		var triangulator = new Triangulator(vertices2D);
		var indices =  triangulator.Triangulate();

		// Generate a color for each vertex
		var colors = Enumerable.Range(0, vertices3D.Length)
			.Select(i => color)
			.ToArray();

		// Create the mesh
		var mesh = new Mesh {
			vertices = vertices3D,
			triangles = indices,
			colors = colors
		};

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		// Set up game object with mesh;
		var meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshRenderer.material = new Material(Shader.Find("Sprites/Default"));

		var filter = gameObject.AddComponent<MeshFilter>();
		filter.mesh = mesh;

		// Add collider
		var collider = gameObject.AddComponent<PolygonCollider2D> ();
		collider.points = vertices2D;
	}

	public float GetLeftHeight(){
		return leftHeight;
	}
		
	public float GetRightHeight(){
		return rightHeight;
	}
}
