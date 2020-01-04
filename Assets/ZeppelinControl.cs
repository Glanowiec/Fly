using UnityEngine;
using System.Collections;

public class ZeppelinControl : UpdateShipPosition {

	// Use this for initialization
	
	public void resetHit() {
	   hitcount = 3;
	}
	
	void Start () {
		resetHit ();
	}
	
		
	// Update is called once per frame
	void Update () {
        //Zeppelin moving to the right
        transform.Translate((Vector3.up * -1));
    }
}