using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Target;

public class Folower : MonoBehaviour
{
    [SerializeField] Camera gameCamera;
    [SerializeField] Target target;
    [SerializeField] GameObject lookToTarget;
    [SerializeField] float speed = 0.01f;

    //hits info
    RaycastHit downHitInfo;
    RaycastHit rayToTargetHitInfo;

    Vector3 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        currentPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;

        //look to target ray
        lookToTarget.transform.LookAt(target.pos);
        Ray rayToTarget = new Ray(lookToTarget.transform.position, lookToTarget.transform.forward);
        Physics.Raycast(rayToTarget, out rayToTargetHitInfo);
        Debug.DrawRay(rayToTarget.origin, rayToTarget.direction * 100, Color.blue);

        Ray downRay = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(downRay, out downHitInfo, 1))
        {
            //if folower and target stay on same object
            if (target.raysDict[Direction.Down].hitInfo.collider.Equals(downHitInfo.collider) && rayToTargetHitInfo.collider.Equals(target.targetCollider))
            {                
                //just move to target
                transform.position = Vector3.MoveTowards(transform.position, target.pos, speed);
            }

            //rotates perpendicular to the ground
            transform.rotation = Quaternion.FromToRotation(Vector3.up, downHitInfo.normal);

            if(Vector3.Distance(currentPos, target.pos) > 0.1f)
            {
                //looks at the target on the x and z axes
                Vector3 xzTargetPos = target.pos;
                xzTargetPos.y = currentPos.y;
                transform.LookAt(xzTargetPos);
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