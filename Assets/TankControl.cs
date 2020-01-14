using UnityEngine;
using System.Collections;

public class TankControl : UpdateShipPosition
{

    // Use this for initialization
    public void resetHit()
    {
        hitcount = 3;
    }

    void Start()
    {

        resetHit();
    }


    // Update is called once per frame
    void Update()
    {
        if (hitcount == 3)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(30, 0, 0);
        }
        else
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }
}
