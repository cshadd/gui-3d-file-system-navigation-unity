using System;
using System.IO;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class CreateObjects : MonoBehaviour
{
    public Text txtSelectedNode;
    public Text txtHoveredOverNode;

    DataNode currentSelectedNode;

    // Start is called before the first frame update
    void Start()
    {

       // txtSelectedNode.text = "";
       // txtHoveredOverNode.text = "";



        float index = 0;
        foreach (var drive in DriveInfo.GetDrives())
        {
            Debug.Log($"Drive: {drive.Name} Root: { drive.RootDirectory}");

            // Create a primitive type cube game object
            var gObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Calculate the position of the game object in the world space
            float x = index + 1f;
            int y = 0;
            int z = 20;

            // Position the game object in world space
            gObj.transform.position = new Vector3(x, y, z);
            gObj.transform.localScale = new Vector3(10, 1, 10);
            gObj.name = drive.Name;

            //color
            var cubeRenderer = gObj.GetComponent<Renderer>();
            cubeRenderer.material.SetColor("_Color", Color.gray);


            // Add DataNode component and update the attributes for later usage
            gObj.AddComponent<DataNode>();
            DataNode dn = gObj.GetComponent<DataNode>();
            dn.Name = drive.Name;
            dn.Size = drive.TotalSize;

            index += 12f;
        }

        // Set a variable to the My Documents path.
       
    }

    RaycastHit hitInfo = new RaycastHit();

    void Update()
    {
        // Check to see if the Left Mouse Button was clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Create a raycase from the screen-space into World Space, store the data in hitInfo Object
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                // if there is a hit, we want to get the DataNode component to extract the information
                DataNode dn = hitInfo.transform.GetComponent<DataNode>();
                txtSelectedNode.text = $"{dn.Name} and {dn.Size}";
            }
        }
    }
}
