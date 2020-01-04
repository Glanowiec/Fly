using UnityEngine;
using System.Collections;

public class MisileControl : MonoBehaviour {

	// Use this for initialization
	GameObject plane;
	
	void Start () {
	  plane = GameObject.Find ("plane");
	}
	
	
	void OnCollisionEnter(Collision col)
	{
		
		if (col.gameObject.name.IndexOf("Battleship") > -1 || col.gameObject.name.IndexOf("zeppelin2") > -1) 
		{			
			//col.gameObject.SendMessageUpwards("OnExplode");
			Debug.Log ("Hit object: "+col.gameObject.name);
			plane.SendMessage("AddPoints",100);
			
		} else
		if (col.gameObject.name.IndexOf ("Bridge") > -1)
		{
			plane.SendMessage("AddPoints",10);
		} else
		if (col.gameObject.name.IndexOf ("tank") > -1)
		{
			plane.SendMessage("AddPoints",200);
		}
		Destroy(this.gameObject);
	}
	
	
	// Update is called once per frame
	void FixedUpdate () {
		if (transform.position.y <= 15)
		{
			Destroy(this.gameObject);
		}
	}
}
