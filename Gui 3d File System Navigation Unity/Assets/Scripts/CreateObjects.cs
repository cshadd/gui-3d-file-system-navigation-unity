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


    void Start()  // CREATES DRIVES
    {
        //txtSelectedNode.text = "";
        //txtHoveredOverNode.text = "";

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
            gObj.transform.localScale = new Vector3(1, 1, 1);
            gObj.name = drive.Name;

            //color
            var cubeRenderer = gObj.GetComponent<Renderer>();
            cubeRenderer.material.SetColor("_Color", Color.gray);

            // Add DataNode component and update the attributes for later usage
            gObj.AddComponent<DataNode>();
            DataNode dn = gObj.GetComponent<DataNode>();
            dn.Name = drive.Name;
            dn.Size = drive.TotalSize;
            dn.FullName = drive.RootDirectory.FullName;
            dn.IsDrive = true;

            index += 12f;
        }

    }

    RaycastHit hitInfo = new RaycastHit();
    void Update()
    {
        #region HANDLE MOUSE INTERACTION
        // Create a raycase from the screen-space into World Space, store the data in hitInfo Object
        bool Hoverhit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (Hoverhit)
        {
            if (hitInfo.transform.GetComponent<DataNode>() != null)
            {
                // if there is a hit, we want to get the DataNode component to extract the information
                DataNode dn = hitInfo.transform.GetComponent<DataNode>();
                txtHoveredOverNode.text = $"{dn.FullName}";
            }
        }
        else
        {
            txtHoveredOverNode.text = $"";
        }
        #endregion

        // Check to see if the Left Mouse Button was clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Create a raycase from the screen-space into World Space, store the data in hitInfo Object
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                var gObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                gObj.transform.position = new Vector3(10 + transform.position.x, 10 + transform.position.y, 10 + transform.position.z);
                gObj.transform.SetParent(transform);
                gObj.transform.LookAt(transform);
                gObj.transform.Translate(Vector3.forward * -(10 % 2), Space.Self);
            }
        }
    }
}
