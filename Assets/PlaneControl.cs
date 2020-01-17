using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class PlaneControl : MonoBehaviour
{


    static DateTime lastFire = DateTime.Now;
    static GameSetings setings = new GameSetings();

    //Starting values
    public static int iscore = 0;
    public static int irockets = 100;
    public static int ihealth = 0;

    //Pad texture
    public Texture padBack;
    public Texture padPoint;
    public Texture fireButton;

    //Missiles Rigibody object (Rigibody - have a physical properties)
    public Rigidbody missile;

    //AddPoint (Score)
    public void AddPoints(int p)
    {
        iscore += p;
    }


    //Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 30);
    }

    //Update is called once per frame
    void Update()
    {
        Vector3 pos = this.transform.position;

        //Time since last shot
        System.TimeSpan lastFireTimeAndNowDiff = DateTime.Now - lastFire;

        //Fire
        if (Input.GetKey(KeyCode.Space) && lastFireTimeAndNowDiff.TotalMilliseconds > 500)
        {
            lastFire = DateTime.Now;
            // count vector forward
            var pointerObject = GameObject.Find("Pointer");
            Vector3 missileVector = pointerObject.transform.position - this.transform.position;
            Rigidbody missileInstance = Instantiate(missile, pointerObject.transform.position, this.transform.rotation) as Rigidbody;
            missileInstance.AddForce(missileVector * 50000);

            irockets--;
        }


        //Plane movement controls
        if (Input.GetKey(KeyCode.UpArrow))
        {
            pos.y -= 1;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            pos.y += 1;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += 1;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= 1;
        }



        float dx = Input.acceleration.x;
        float dy = Input.acceleration.y;
        pos.x = pos.x + dx * Time.deltaTime * 20;
        pos.y = pos.y + dy * Time.deltaTime * 20;

        //Movement limits 
        if (pos.x > 1065) pos.x = 1065;
        else if (pos.x < 930) pos.x = 930;
        if (pos.y > 100) pos.y = 100;
        else if (pos.y < 40) pos.y = 40;

        //New place position
        transform.rotation = Quaternion.Euler(270 - 90 * dx, 270, 0);
        this.transform.position = pos;

    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("You died: " + other.name);
    }


    //ex5
    void OnCollisionEnter(Collision collision)
    {
        SceneManager.LoadScene("gameOver");
    }

    void OnGUI()
    {

        if (this.GetComponent<Renderer>().enabled)
        {

            // count vector forward
            // var obj = GameObject.Find("SphereShipFront");
            // Vector3 v = obj.transform.position - this.transform.position;

            //Display fire button
            Rect rf = new Rect(30, Screen.height - 90, 60, 60);
            GUI.DrawTexture(rf, fireButton, ScaleMode.ScaleToFit, true);

            //Display pad background
            Rect r = new Rect(Screen.width - 120, Screen.height - 120, 100, 100);
            GUI.DrawTexture(r, padBack, ScaleMode.StretchToFill, true, 0f);
            int px = Screen.width - 120 + 50; ;
            int py = Screen.height - 120 + 50;
            float padx = Screen.width - 120 + 50;
            float pady = Screen.height - 120 + 50;

            //Check multitouch
            if (Input.touchCount > 0)
                for (int i = 0; i < Input.touchCount; i++)
                {
                    float tx = Input.GetTouch(i).position.x;
                    float ty = Screen.height - Input.GetTouch(i).position.y;
                    //Check if virtual pad is touched
                    if ((px - tx) * (px - tx) + (py - ty) * (py - ty) <= 4900)
                    {
                        padx = tx;
                        pady = ty;
                        // update position
                        float dx = (padx - px) / 100;
                        float dy = (pady - py) / 100;

                        // rotate plane	
                        transform.rotation = Quaternion.Euler(270 - padx + px, 270, 0);
                        Vector3 pos = this.transform.position;

                        pos.x = pos.x + dx;
                        pos.y = pos.y + dy;
                        if (pos.x > 1065) pos.x = 1065;
                        else if (pos.x < 930) pos.x = 930;
                        if (pos.y > 100) pos.y = 100;
                        else if (pos.y < 40) pos.y = 40;

                        this.transform.position = pos;
                    }
                    else
                    {

                    }

                    //check if fire is pressed
                    System.TimeSpan ts = System.DateTime.Now - lastFire;
                    if ((rf.xMax > tx) && (rf.xMin < tx) && (rf.yMax > ty) && (rf.yMin < ty) && (ts.TotalMilliseconds >= 1000))
                    {
                        if (irockets > 0)
                        {
                            lastFire = DateTime.Now;
                            // count vector forward
                            var obj = GameObject.Find("Pointer");
                            Vector3 v = obj.transform.position - this.transform.position;
                            Rigidbody instance = Instantiate(missile, obj.transform.position, this.transform.rotation) as Rigidbody;
                            instance.AddForce(v * 50000);

                            irockets--;
                        }
                    }
                }
            // draw touch pad point	
            GUI.DrawTexture(new Rect(padx - 7, pady - 7, 15, 15), padPoint, ScaleMode.ScaleToFit, true, 0f);
            if (padx - px == 0) transform.rotation = Quaternion.Euler(270, 270, 0);

            //Display score health and rockets
            GUIStyle myStyle = GUI.skin.label;
            myStyle.fontSize = 14;
            myStyle.normal.textColor = Color.blue;
            GUI.Label(new Rect(40, 20, 200, 30), "Score: " + iscore, myStyle);
            GUI.Label(new Rect(Screen.width / 2 - 50, 20, 200, 30), "Rockets: " + irockets, myStyle);
            GUI.Label(new Rect(Screen.width - 130, 20, 100, 30), "P: " + Input.acceleration.normalized.x, myStyle);
        }

    }
}
