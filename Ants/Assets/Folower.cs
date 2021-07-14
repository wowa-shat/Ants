using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folower : MonoBehaviour
{
    [SerializeField] Camera gameCamera;
    [SerializeField] Target target;

    RaycastHit downHitInfo;

    Vector3 currentPos;
    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        currentPos = transform.position;
        targetPos = target.targetPos;
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;
        targetPos = target.targetPos;

        Ray downRay = new Ray(transform.position, -transform.up);
        if(Physics.Raycast(downRay, out downHitInfo, 1))
        {
            //if folower and target stay on same object
            if (target.oRays[0].hitInfo.collider.Equals(downHitInfo.collider))
            {                
                //just move to target
                transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.05f);
            }

            //rotates perpendicular to the ground
            transform.rotation = Quaternion.FromToRotation(Vector3.up, downHitInfo.normal);

            if(Vector3.Distance(currentPos, targetPos) > 0.5f)
            {
                //looks at the target on the x and z axes
                targetPos.y = currentPos.y;
                transform.LookAt(targetPos);
            }

            print("Ant down hit: " + downHitInfo.collider.name);
        }
        Debug.DrawRay(downRay.origin, downRay.direction, Color.red);

        //Ray forwardRay = new Ray(transform.position, transform.forward);
        //RaycastHit forwardHitInfo;
        //Physics.Raycast(forwardRay, out forwardHitInfo, 1);
        //Debug.DrawRay(forwardRay.origin, forwardRay.direction, Color.red);
    }
}