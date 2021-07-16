using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{    public enum Direction { Down, Right, Left, Forward, Back };

    [SerializeField] Camera gameCamera;
    [SerializeField] Transform targetMesh;
    [SerializeField] LayerMask targetMask;

    public Dictionary<Direction, OriginRay> raysDict { get; private set; } = new Dictionary<Direction, OriginRay>();

    public Vector3 pos { get; private set; }
    public Collider targetCollider { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        UpdateRays();
        pos = targetMesh.position;
        targetCollider = targetMesh.gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray mouseRay = gameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit mouseRayHitInfo;

        //replace target
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(mouseRay, out mouseRayHitInfo, 1000, targetMask))
            {
                transform.position = mouseRayHitInfo.point;
                pos = targetMesh.position;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, mouseRayHitInfo.normal);

                UpdateRays();
            }
        }

        if(Physics.Raycast(raysDict[Direction.Down].ray, out raysDict[Direction.Down].hitInfo))
        {
            print("Target down hit: " + raysDict[0].hitInfo.collider.name);
        }

        //debug
        foreach (OriginRay oRay in raysDict.Values)
        {
            Debug.DrawRay(oRay.ray.origin, oRay.ray.direction, Color.red);
        }
    }

    private void UpdateRays()
    {
        //update rays
        raysDict[Direction.Down] = new OriginRay(new Ray(targetMesh.position, -targetMesh.up));
        //oRays[Direction.Right] = new OriginRay(Direction.Right, new Ray(targetMesh.position, targetMesh.right));
        //oRays[Direction.Left] = new OriginRay(Direction.Left, new Ray(targetMesh.position, -targetMesh.right));
        //oRays[Direction.Forward] = new OriginRay(Direction.Forward, new Ray(targetMesh.position, targetMesh.forward));
        //oRays[Direction.Back] = new OriginRay(Direction.Back, new Ray(targetMesh.position, -targetMesh.forward));
    }

    public class OriginRay
    {
        public Ray ray;
        public RaycastHit hitInfo;

        public OriginRay(Ray ray)
        {
            this.ray = ray;

            Physics.Raycast(this.ray, out hitInfo, 1);
        }
    }
}
