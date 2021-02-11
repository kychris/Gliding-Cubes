using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Friction : MonoBehaviour
{
    public float velocityGlideFriction = 1f;
    public float angularGlideFriction = 0.7f;

    Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        // Get Rigidbody of current object
        if (m_Rigidbody == null)
            m_Rigidbody = GetComponent<Rigidbody>();
        if (m_Rigidbody == null)
            Debug.LogWarning("Grab Interactable does not have a required Rigidbody.", this);
    }

    // Begin to apply friction to object's current velocity
    public void StartApplyFriction()
    {
        StartCoroutine("ApplyVelocityFriction");
        StartCoroutine("ApplyAngularFriction");
    }

    // Apply friction on position transform
    IEnumerator ApplyVelocityFriction()
    {
        float multiplier;
        float tParam = 0.0f;
        Vector3 detachVelocity = m_Rigidbody.velocity;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * velocityGlideFriction;

            // Lerp and smoothstep to get continously decreasing multiplier
            multiplier = Mathf.Lerp(0.0f, 1.0f, 1 - Mathf.SmoothStep(0.0f, 1.0f, tParam));

            // Apply multiplier to velocity
            m_Rigidbody.velocity = multiplier * detachVelocity;

            yield return null;
        }
    }

    // Apply friction on rotation transform
    IEnumerator ApplyAngularFriction()
    {
        float multiplier;
        float tParam = 0.0f;
        Vector3 detachAngularVelocity = m_Rigidbody.angularVelocity;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * angularGlideFriction;

            // Lerp and smoothstep to get continously decreasing multiplier
            multiplier = Mathf.Lerp(0.0f, 1.0f, 1 - Mathf.SmoothStep(0.0f, 1.0f, tParam));

            // Apply multiplier to velocity
            m_Rigidbody.angularVelocity = multiplier * detachAngularVelocity;

            yield return null;
        }
    }
}
