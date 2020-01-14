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
            //exp.parent = this.gameObject.transform;
            //smo.parent = this.gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
