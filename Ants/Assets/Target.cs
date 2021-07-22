using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{    public enum Direction { Down, Right, Left, Forward, Back };

    [SerializeField] Camera gameCamera;
    [SerializeField] Transform targetMesh;
    [SerializeField] LayerMask targetMask;

    public Vector3 pos { get; private set; }
    public Collider targetCollider { get; private set; }

    void Start()
    {
        pos = targetMesh.position;
        targetCollider = targetMesh.gameObject.GetComponent<BoxCollider>();
    }

    void Update()
    {
        //replace target
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = gameCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseRayHitInfo;
            if (Physics.Raycast(mouseRay, out mouseRayHitInfo, 1000, targetMask))
            {
                transform.position = mouseRayHitInfo.point;
                pos = targetMesh.position;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, mouseRayHitInfo.normal);
            }
        }
    }
}
