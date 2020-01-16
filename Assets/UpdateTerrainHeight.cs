using UnityEngine;
using System.Collections;
using System;

public class UpdateTerrainHeight : MonoBehaviour
{

    public Terrain TerrainN, TerrainC, TerrainS;
    public Transform Ship;
    public Transform zeppelin;
    private DateTime lastZeppelinCreated = DateTime.Now;

    //Objects Pools
    public static Pool pulaZeppelin = new Pool();
    public static Pool pulaShip = new Pool();

    Queue onScene = new Queue();
    int interval = 1;
    Camera camera;

    static DateTime dt = DateTime.Now;
    static DateTime dtNewShip = DateTime.Now;
    GameObject bridge;

    // Use this for initialization
    void Start()
    {
        pulaZeppelin.pooledObject = zeppelin;
        pulaZeppelin.MaxCount = 10;
        pulaZeppelin.Init();

        pulaShip.pooledObject = Ship;
        pulaShip.MaxCount = 7;
        pulaShip.Init();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        camera = (Camera)GetComponent("Camera");
        Camera.main.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 30);

        bridge = GameObject.Find("Bridge");

        editTerrain(TerrainC);
        //AddShips(TerrainC);
        editTerrain(TerrainN);
        //AddShips(TerrainN);
    }





    void AddShip()
    {
        //l - this variable is random and by this value we provide object from pools.
        int l = UnityEngine.Random.Range(1, 10);
        float cameraZPosition = camera.transform.position.z + 1100;
        if (l <= 3)
        {
            //Add zeppelin
            Transform zeppelinObjectFromPool = pulaZeppelin.getObject();
            zeppelinObjectFromPool.transform.position = new Vector3(UnityEngine.Random.Range(900, 1100), UnityEngine.Random.Range(30, 70), cameraZPosition);
            zeppelinObjectFromPool.Rotate(new Vector3(-90.0f, 0f, 270.0f));
            onScene.Enqueue(zeppelinObjectFromPool);

        }
        else
        if (l < 7)
        {
            //Add ship
            Transform shipObjectFromPool = pulaShip.getObject();
            shipObjectFromPool.transform.position = new Vector3(UnityEngine.Random.Range(950, 1050), 15, cameraZPosition);
            shipObjectFromPool.Rotate(new Vector3(270.0f, UnityEngine.Random.Range(0, 180.0f), 0.0f));
            onScene.Enqueue(shipObjectFromPool);
        }
        else
        {
            if (bridge.transform.position.z < camera.transform.position.z)
            {
                GameObject tank = GameObject.Find("Bridge/tank");
                tank.SendMessage("resetHit");
                bridge.transform.position = new Vector3(1000, UnityEngine.Random.Range(40, 80), cameraZPosition);
                Vector3 vectorBridge = bridge.transform.position;
                vectorBridge.x = 600;
                tank.transform.position = vectorBridge;
                tank.GetComponent<Rigidbody>().velocity = new Vector3(5, 0, 0);
            }
            else
            {
                Transform obj = pulaShip.getObject();
                obj.transform.position = new Vector3(UnityEngine.Random.Range(950, 1050), 15, cameraZPosition);

                obj.Rotate(new Vector3(270.0f, UnityEngine.Random.Range(0, 180.0f), 0.0f));
                onScene.Enqueue(obj);
            }
        }
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        TimeSpan ts = DateTime.Now - dtNewShip;
        if (ts.TotalSeconds > interval)
        {
            AddShip();
            dtNewShip = DateTime.Now;
            interval = UnityEngine.Random.Range(7, 10);
        }
        UpdatePos();

    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
            {
                Application.Quit();
                return;
            }
        }
    }

    void UpdatePos()
    {
        TimeSpan ts = DateTime.Now - dt;
        if (ts.TotalMilliseconds > 1000)
        {
            Vector3 v = camera.transform.position;
            dt = DateTime.Now;

            // check if terrain movement is needed
            if (camera.transform.position.z >= (TerrainC.transform.position.z + 1000))
            {
                //editTerrain(TerrainS);
                //DeleteShips(TerrainC);
                //AddShips(TerrainC);
                v = TerrainC.transform.position;
                v.z += 1000 * 2;
                Terrain tempTerrain = TerrainC;
                tempTerrain.transform.position = v;

                TerrainC = TerrainN;
                TerrainN = tempTerrain;

            }

            // Check object onScene
            if (onScene.Count > 0)
            {
                Transform obj = (Transform)onScene.Peek();

                if (obj != null)
                {
                    if (camera.transform.position.z > obj.transform.position.z)
                    {
                        if (obj.name.IndexOf("zeppelin") > -1)
                            pulaZeppelin.putObject((Transform)onScene.Dequeue());
                        if (obj.name.IndexOf("Battleship") > -1)
                            pulaShip.putObject((Transform)onScene.Dequeue());
                    }
                }
            }
        }
    }


    public void CreateHill(int x, int y, float height, float pointyness, float[,] terrainGRID)
    {
        float point = 0;
        float distanceFromTop;

        terrainGRID[x, y] = height;

        for (int a = 0; a < 129; a++)
        {
            for (int b = 0; b < 129; b++)
            {
                distanceFromTop = Mathf.Sqrt(Mathf.Pow((y - b), 2) + Mathf.Pow((x - a), 2));
                point = ((height - terrainGRID[a, b]) * 1000 - distanceFromTop * pointyness) / 1000;

                if (point < 0)
                    point = 0;

                terrainGRID[a, b] += point;
            }
        }
    }



    private void Smooth(float[,] height)
    {

        float k = 0.8f;
        /* Rows, left to right */
        for (int x = 1; x < 129; x++)
            for (int z = 0; z < 129; z++)
                height[x, z] = height[x - 1, z] * (1 - k) +
                          height[x, z] * k;

        /* Rows, right to left*/
        for (int x = 129 - 2; x < -1; x--)
            for (int z = 0; z < 129; z++)
                height[x, z] = height[x + 1, z] * (1 - k) +
                          height[x, z] * k;

        /* Columns, bottom to top */
        for (int x = 0; x < 129; x++)
            for (int z = 1; z < 129; z++)
                height[x, z] = height[x, z - 1] * (1 - k) +
                          height[x, z] * k;

        /* Columns, top to bottom */
        for (int x = 0; x < 129; x++)
            for (int z = 129; z < -1; z--)
                height[x, z] = height[x, z + 1] * (1 - k) +
                          height[x, z] * k;

    }


    void editTerrain(Terrain myTerrain)
    {
        int xResolution = myTerrain.terrainData.heightmapWidth;
        int zResolution = myTerrain.terrainData.heightmapHeight;


        float[,] heights = myTerrain.terrainData.GetHeights(0, 0, xResolution, zResolution);
        //PerlinTerrain (heights, xResolution, zResolution);

        //for (int i=0;i<20;i++)
        CreateHill(25, 25, 0.4f, 15f, heights);
        //CreateHill(50, 25, 0.4f, 15f , heights);
        CreateHill(75, 25, 0.4f, 15f, heights);
        //CreateHill(100, 25, 0.4f, 15f , heights);
        CreateHill(125, 25, 0.4f, 15f, heights);

        //CreateHill(25, 100, 0.4f, 15f , heights);
        CreateHill(50, 100, 0.4f, 15f, heights);
        //CreateHill(75, 100, 0.4f, 15f , heights);
        CreateHill(100, 100, 0.4f, 15f, heights);
        //CreateHill(125, 100, 0.4f, 15f , heights);

        //Smooth(heights);
        myTerrain.terrainData.SetHeights(0, 0, heights);
    }

    public Pool getZeppelinPool() 
    {
        return pulaZeppelin;
    }
}
