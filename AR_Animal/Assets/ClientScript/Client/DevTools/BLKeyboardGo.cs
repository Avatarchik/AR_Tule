using UnityEngine;
using System.Collections;

public class BLKeyboardGo : MonoBehaviour
{
    public float _Speed = 10f;
    Camera Cam;
	// Use this for initialization
	void Start () {

        Cam = GetComponentInChildren<Camera>();

        if (Cam.GetComponent<BLMouseLook>() == null)
        {
            Cam.gameObject.AddComponent<BLMouseLook>();
        }
        
	}
	
	// Update is called once per frame
	void Update () {

        float Speed = _Speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Cam.transform.forward * Speed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Cam.transform.forward * -Speed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Cam.transform.right * Speed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Cam.transform.right * -Speed);
        }
        
	}
}
