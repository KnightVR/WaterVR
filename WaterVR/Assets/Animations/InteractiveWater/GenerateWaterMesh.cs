using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWaterMesh : MonoBehaviour
{
	public static List<Vector3[]> GenerateWater(MeshFilter waterMeshFilter, float size, float spacing)
	{
		//Determine the number of vertices per row/column (is always a square)
		int totalVertices = (int)Mathf.Round(size / spacing) + 1;

		//Vertices 2d list
		List<Vector3[]> vertices2dArray = new List<Vector3[]>();
		//Triangles
		List<int> tris = new List<int>();

		for (int z = 0; z < totalVertices; z++)
		{
			vertices2dArray.Add(new Vector3[totalVertices]);

			for (int x = 0; x < totalVertices; x++)
			{
				//Fill this array every loop
				Vector3 current_point = new Vector3();

				//Get the corrdinates of the vertice
				current_point.x = x * spacing;
				current_point.z = z * spacing;
				current_point.y = 0f; //assume always at 0

				vertices2dArray[z][x] = current_point;

				//Don't generate a triangle the first coordinate on each row
				if (x <= 0 || z <= 0)
				{
					continue;
				}

				//Build the triangles
				//Each square consists of 2 triangles

				//The triangle south-west of the vertice
				tris.Add(x + z * totalVertices);
				tris.Add(x + (z - 1) * totalVertices);
				tris.Add((x - 1) + (z - 1) * totalVertices);

				//The triangle west-south of the vertice
				tris.Add(x + z * totalVertices);
				tris.Add((x - 1) + (z - 1) * totalVertices);
				tris.Add((x - 1) + z * totalVertices);
			}
		}

		//Unfold the 2d array of verticies into a 1d array.
		Vector3[] unfolded_verts = new Vector3[totalVertices * totalVertices];
		for (int i = 0; i < vertices2dArray.Count; i++)
		{
			//Copies all the elements of the current array to the specified array
			vertices2dArray[i].CopyTo(unfolded_verts, i * totalVertices);
		}

		//Generate the mesh
		Mesh waterMesh = new Mesh();
		waterMesh.vertices = unfolded_verts;
		waterMesh.triangles = tris.ToArray();
		//Ensure the bounding volume is correct
		waterMesh.RecalculateBounds();
		//Update the normals to reflect the change
		waterMesh.RecalculateNormals();
		waterMesh.name = "WaterMesh";


		//Add the generated mesh to the GameObject
		waterMeshFilter.mesh.Clear();
		waterMeshFilter.mesh = waterMesh;

		//Return the 2d array
		return vertices2dArray;
	}
}
