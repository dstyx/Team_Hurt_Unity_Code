using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover : MonoBehaviour {

    public Renderer rend;

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
	}

    void OnMouseEnter()
    {
        rend.material.color = Color.green;
    }

    void OnMouseExit()
    {
        rend.material.color = Color.red;
    }

    
	
	// Update is called once per frame
	void Update () {
		
	}
}
