using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    public Camera m_Camera; // Assign in Inspector


    void Update()
    {
        if (m_Camera != null)
        {
            Transform t = m_Camera.transform;
            transform.LookAt(transform.position + t.rotation * Vector3.forward,
                               t.rotation * Vector3.up);
        }


    }
}
