using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


// File for testing components without VR
public class Tester : MonoBehaviour
{
    public Transform range;
    GameObject[] interactables;
    public Material selectedMaterial;

    // Start is called before the first frame update
    void Start()
    {


        // interactables = GameObject.FindGameObjectsWithTag("Interactable");

        // foreach (GameObject o in interactables)
        // {
        //     Debug.Log(o.name);
        //     Debug.Log(o.transform.position);
        //     o.transform.SetParent(range);
        //     o.GetComponent<Renderer>().material = selectedMaterial;
        //     o.GetComponent<XRGrabInteractable>().enabled = false;
        // }

        Rigidbody rb = range.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // interactables = GameObject.FindGameObjectsWithTag("Interactable");

        // foreach (GameObject o in interactables)
        // {
        //     Debug.Log(o.name);
        //     Debug.Log(o.transform.position);
        //     // o.transform.SetParent(range);
        //     o.transform.SetParent(null);
        // }
    }
}
