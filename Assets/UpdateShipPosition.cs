using UnityEngine;
using System.Collections;

public class UpdateShipPosition : MonoBehaviour
{

    // explosion particle
    public Transform explosion1;
    public Transform smoke;

    // Use this for initialization
    public int hitcount = 3;

    void Start()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.IndexOf("Missile") > -1)
        {
            Debug.Log("ship explosion");
            hitcount--;
            Transform exp = Instantiate(explosion1, collision.contacts[0].point, Quaternion.identity) as Transform;
            Transform smo = Instantiate(smoke, collision.contacts[0].point, Quaternion.identity) as Transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //ex4
        //If 3 shots - remove ship from scene and put in Pool
        if (hitcount <= 0 && transform.position != Vector3.zero)
        {
            UpdateTerrainHeight.pulaShip.putObject(transform);
            hitcount = 3;
        }
    }
}
