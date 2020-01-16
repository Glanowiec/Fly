using UnityEngine;

public class ZeppelinControl : UpdateShipPosition {

	private Vector3 startPosition;
	private int speed = 40;

	// Use this for initialization
	public void resetHit() {
		hitcount = 3;
	}
	
	void Start () {
		resetHit ();
	}


	// Update is called once per frame
	//Here we are checking if zeppelin is out of our sight (Axis Y). If not - it will flight again from right.
	//INFO: it can be checked by change speed.
	void Update () {

		if (hitcount > 0)
		{
			if (transform.position != Vector3.zero && transform.position.x > 100)
			{
				//deltaTime - time diff between frames. Control speed of zeppelin
				transform.Translate(Vector3.up * Time.deltaTime * speed * -1);
			}
			else if (transform.position != Vector3.zero && transform.position.x < 100)
			{
				transform.position = new Vector3(1600, transform.position.y, transform.position.z);
			}
		}
		//If 3 shots - remove zeppelin from scene and put in Pool
		else if (hitcount <= 0 && transform.position != Vector3.zero)
		{
			UpdateTerrainHeight.pulaZeppelin.putObject(transform);
			hitcount = 3;
		}
	}

}