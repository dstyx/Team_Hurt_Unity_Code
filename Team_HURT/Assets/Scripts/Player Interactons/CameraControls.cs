using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {

    public float vCamSpeed=5.0f;
    public float hCamSpeed = 10f;
    public float rCamSpeed = 500f;

    
    

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * vCamSpeed;
            transform.position += transform.up * Time.deltaTime * vCamSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Time.deltaTime * vCamSpeed;
            transform.position -= transform.up * Time.deltaTime * vCamSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * hCamSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * Time.deltaTime * hCamSpeed;
        }

        if (Input.GetKey(KeyCode.E))
        {

            // transform.Rotate(Vector3.up * Time.deltaTime* rCamSpeed, Space.World);
            transform.Rotate(Vector3.up, 30 * Time.deltaTime * rCamSpeed, Space.World);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            //transform.Rotate(Vector3.up * Time.deltaTime * -rCamSpeed, Space.World);
            transform.Rotate(Vector3.up, 30 * Time.deltaTime * -rCamSpeed, Space.World);
        }


        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        

	}
}
