using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Target;

public class Folower : MonoBehaviour
{
    [SerializeField] Camera gameCamera;
    [SerializeField] Target target;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform lookToTarget;
    [SerializeField] Transform nextStep;
    [SerializeField] float nextStepAngle = 80f;

    //hits info
    RaycastHit downHitInfo;
    RaycastHit rayToTargetHitInfo;
    RaycastHit nextStepHitInfo;

    Vector3 currentPos;
    float distanceToTarget;

    // Start is called before the first frame update
    void Start()
    {
        currentPos = transform.position;
        nextStep.Rotate(new Vector3(1, 0, 0), nextStepAngle);
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;
        distanceToTarget = Vector3.Distance(currentPos, target.pos);

        //next step raycast
        Ray nextStepRay = new Ray(nextStep.position, nextStep.forward);
        Physics.Raycast(nextStepRay, out nextStepHitInfo, 1, groundMask);
        Debug.DrawRay(nextStepRay.origin, nextStepRay.direction, Color.green);

        //look to target ray
        lookToTarget.LookAt(target.pos);
        Ray rayToTarget = new Ray(lookToTarget.position, lookToTarget.forward);
        Physics.Raycast(rayToTarget, out rayToTargetHitInfo);
        Debug.DrawRay(rayToTarget.origin, rayToTarget.direction * 100, Color.blue);

        Ray downRay = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(downRay, out downHitInfo, 1))
        {
            //if folower and target stay on same object
            if (target.raysDict[Direction.Down].hitInfo.collider.Equals(downHitInfo.collider) && rayToTargetHitInfo.collider.Equals(target.targetCollider))
            {
                if (distanceToTarget > 0.1f)
                {
                    transform.position = nextStepHitInfo.point;
                }
                transform.position = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, transform.position.z);
            }

            //rotates perpendicular to the ground
            transform.rotation = Quaternion.FromToRotation(Vector3.up, downHitInfo.normal);

            if(distanceToTarget > 0.1f)
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