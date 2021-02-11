using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CubeMesh : MonoBehaviour
{
    public Transform rightHandPos;
    public Transform leftHandPos;
    public Transform meshLocation;

    bool shouldCreate = false;

    void Start() { }

    void Update()
    {
        // Only create mesh when buttons are pressed
        if (shouldCreate)
        {
            CreateCubeMesh();
        }
    }

    private void CreateCubeMesh()
    {
        Vector3 rightHandRelative = rightHandPos.InverseTransformPoint(transform.position);

        Vector3[] vertices = {
            new Vector3 (leftHandPos.position.x, leftHandPos.position.y, leftHandPos.position.z),
            new Vector3 (rightHandPos.position.x, leftHandPos.position.y, leftHandPos.position.z),
            new Vector3 (rightHandPos.position.x, rightHandPos.position.y, leftHandPos.position.z),
            new Vector3 (leftHandPos.position.x, rightHandPos.position.y, leftHandPos.position.z),
            new Vector3 (leftHandPos.position.x, rightHandPos.position.y, rightHandPos.position.z),
            new Vector3 (rightHandPos.position.x, rightHandPos.position.y, rightHandPos.position.z),
            new Vector3 (rightHandPos.position.x, leftHandPos.position.y, rightHandPos.position.z),
            new Vector3 (leftHandPos.position.x, leftHandPos.position.y, rightHandPos.position.z),
        };

        /*
        Vertex Numbering

         / 4---5
        3---2  |
        |  7|--6
        0---1 /

        */

        // Create triangles for the vertexes
        int[] triangles = {
            0, 2, 1, // face front 
            1, 2, 0, // (all double sided)
            0, 3, 2,
            2, 3, 0,
            2, 3, 4, // face top
            4, 3, 2,
            2, 4, 5,
            5, 4, 2,
            1, 2, 5, // face right
            5, 2, 1,
            1, 5, 6,
            6, 5, 1,
            0, 7, 4, // face left
            4, 7, 0,
            0, 4, 3,
            3, 4, 0,
            5, 4, 7, // face back
            7, 4, 5,
            5, 7, 6,
            6, 7, 5,
            0, 6, 7, // face bottom
            7, 6, 0,
            0, 1, 6,
            6, 1, 0,
        };

        // Create mesh from vertices and triangles
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.Optimize();
        mesh.RecalculateNormals();
    }

    // Whether cube mesh should be drawn
    public void SetRangeStatus(bool isPressed)
    {
        shouldCreate = isPressed;
    }

    // Get current location of the mesh
    public void GetCurrentCoordinates(out Vector3 leftPos, out Vector3 rightPos)
    {
        leftPos = leftHandPos.position;
        rightPos = rightHandPos.position;
    }
}

