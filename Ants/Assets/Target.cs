using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public enum Direction { Down, Right, Left, Forward, Back };

    [SerializeField] Camera gameCamera;
    [SerializeField] Transform targetMesh;

    public Vector3 targetPos { get; private set; }
    public OriginRay[] oRays { get; private set; } = new OriginRay[5];

    // Start is called before the first frame update
    void Start()
    {
        UpdateOriginRays();
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
                transform.position = hitInfo.point;
                targetPos = targetMesh.position;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

                UpdateOriginRays();
            }
        }

        if(Physics.Raycast(oRays[0].ray, out oRays[0].hitInfo))
        {
            print("Target down hit: " + oRays[0].hitInfo.collider.name);
        }

        //debug
        foreach (OriginRay oRay in oRays)
        {
            Debug.DrawRay(oRay.ray.origin, oRay.ray.direction, Color.red);
        }
    }

    public struct OriginRay
    {
        public Direction direction;
        public Ray ray;
        public RaycastHit hitInfo;

        public OriginRay(Direction direction, Ray ray)
        {
            this.direction = direction;
            this.ray = ray;

            Physics.Raycast(this.ray, out hitInfo, 1);
        }
    }

    private void UpdateOriginRays()
    {
        //update rays
        oRays[0] = new OriginRay(Direction.Down, new Ray(targetMesh.position, -targetMesh.up));
        oRays[1] = new OriginRay(Direction.Right, new Ray(targetMesh.position, targetMesh.right));
        oRays[2] = new OriginRay(Direction.Left, new Ray(targetMesh.position, -targetMesh.right));
        oRays[3] = new OriginRay(Direction.Forward, new Ray(targetMesh.position, targetMesh.forward));
        oRays[4] = new OriginRay(Direction.Back, new Ray(targetMesh.position, -targetMesh.forward));
    }
}
