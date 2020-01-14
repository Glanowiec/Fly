using UnityEngine;

public class ZeppelinControl : UpdateShipPosition {

	private Vector3 startPosition;
	private bool zeppelinAppeared;


	// Use this for initialization
	public void resetHit() {
		hitcount = 3;
		zeppelinAppeared = false;
	}
	
	void Start () {
		resetHit ();
	}
	
		
	// Update is called once per frame
	void Update () {
		//Zeppelin moving to the right
		//if (counter < 100)
		//{
		//	transform.Translate(Vector3.up * -1);
		//	counter++;
		//}
		//else
		//{
		//	transform.Translate(Vector3.up * 100);
		//	counter = 0;
		//}

		//if (transform.position != Vector3.zero && transform.rotation != Quaternion.identity && !zeppelinAppeared) ;
		//{
		//	startPosition = transform.position;
		//	zeppelinAppeared = true;
		//} else if (transform.position != Vector3.zero && transform.rotation != Quaternion.identity && zeppelinAppeared)
		//{
		//	transform.Translate(Vector3.up * -1);
		//} else
		//{
		//	zepp
		//}

	}

}