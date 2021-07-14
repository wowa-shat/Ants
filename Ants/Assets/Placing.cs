using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placing : MonoBehaviour
{
    public Transform objectToPlace;
    public Camera gameCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                objectToPlace.position = hitInfo.point;
                //objectToPlace.position = hitInfo.point = Vector3.MoveTowards(objectToPlace.position, hitInfo.point, 50);
                
                objectToPlace.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            }
        }
    }
}
