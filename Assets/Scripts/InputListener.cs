using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;
using System;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;

public class InputListener : MonoBehaviour
{
    public XRNode leftControllerNode;
    public XRNode rightControllerNode;
    public CubeMesh cubeMesh;
    public Transform range;

    // Materials to show selection
    public Material defaultMaterial;
    public Material selectedMaterial;

    List<InputDevice> leftNodeDevices;
    List<InputDevice> rightNodeDevices;

    // Whether box selection is enabled
    bool isRangeTriggered = false;

    // Awake will be called even if script is disabled
    private void Awake()
    {
        leftNodeDevices = new List<InputDevice>();
        rightNodeDevices = new List<InputDevice>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetDevice();
    }

    // Update is called once per frame
    void Update()
    {
        GetDevice();

        // If no controller connected, don't listen to input
        if (leftNodeDevices.Count == 0 || rightNodeDevices.Count == 0) return;

        bool leftPressed;
        bool rightPressed;

        // Get current status of second button press
        if (leftNodeDevices[0].TryGetFeatureValue(CommonUsages.secondaryButton, out leftPressed)
            && rightNodeDevices[0].TryGetFeatureValue(CommonUsages.secondaryButton, out rightPressed))
        {
            // If both buttons are pressed, enable box selection tool
            if (leftPressed && rightPressed)
            {
                // Deselect any previous cubes
                DeselectRange();

                if (!isRangeTriggered)
                {
                    isRangeTriggered = true;

                    // Initiate the cube range
                    cubeMesh.SetRangeStatus(true);
                }
            }
            // If buttons released, disable box selection tool
            else if (isRangeTriggered)
            {
                isRangeTriggered = false;

                Vector3 leftHandPos;
                Vector3 rightHandPos;

                // Get the range location
                cubeMesh.GetCurrentCoordinates(out leftHandPos, out rightHandPos);

                // Set the mesh location to the newly generated position
                range.transform.position = new Vector3(
                    (leftHandPos.x + rightHandPos.x) / 2,
                    (leftHandPos.y + rightHandPos.y) / 2,
                    (leftHandPos.z + rightHandPos.z) / 2
                );
                Debug.Log("Cube mesh location" + range.position);

                // Select all objects within range location
                SelectRange(leftHandPos, rightHandPos);

                // Stop updating the range
                cubeMesh.SetRangeStatus(false);
            }
        }
    }

    // Get XR controllers associated with the nodes
    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(leftControllerNode, leftNodeDevices);
        InputDevices.GetDevicesAtXRNode(rightControllerNode, rightNodeDevices);
    }

    // Deselect currently selected cubes
    void DeselectRange()
    {
        GameObject[] interactables;
        interactables = GameObject.FindGameObjectsWithTag("Interactable");

        // Take all interactables out of range (can be improved by choosing only ones selected)
        foreach (GameObject o in interactables)
        {
            o.transform.SetParent(null);
            o.GetComponent<Renderer>().material = defaultMaterial;
        }
    }

    // Select cubes encapsulated in the range
    void SelectRange(Vector3 leftHandPos, Vector3 rightHandPos)
    {
        float[] xRange = new float[] {
            Math.Min(leftHandPos.x, rightHandPos.x),
            Math.Max(leftHandPos.x, rightHandPos.x)
        };

        float[] yRange = new float[] {
            Math.Min(leftHandPos.y, rightHandPos.y),
            Math.Max(leftHandPos.y, rightHandPos.y)
        };

        float[] zRange = new float[] {
            Math.Min(leftHandPos.z, rightHandPos.z),
            Math.Max(leftHandPos.z, rightHandPos.z)
        };

        GameObject[] interactables;
        interactables = GameObject.FindGameObjectsWithTag("Interactable");
        foreach (GameObject o in interactables)
        {
            // If cube within range
            Vector3 pos = o.transform.position;
            if (
                pos.x >= xRange[0] &&
                pos.x <= xRange[1] &&
                pos.y >= yRange[0] &&
                pos.y <= yRange[1] &&
                pos.z >= zRange[0] &&
                pos.z <= zRange[1]
                )
            {
                // Set cube parent to range mesh and change the material
                o.transform.SetParent(range);
                o.GetComponent<Renderer>().material = selectedMaterial;
            }
        }
    }
}
