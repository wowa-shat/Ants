using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folower : MonoBehaviour
{
    [SerializeField] Target target;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform lookToTarget;
    [SerializeField] Transform nextStep;
    [SerializeField] Transform mesh;

    //hits info
    RaycastHit downHitInfo;
    RaycastHit rayToTargetHitInfo;
    RaycastHit nextStepHitInfo;

    float distanceToTarget;

    void FixedUpdate()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.pos);

        //next step raycast
        Ray nextStepRay = new Ray(nextStep.position, -nextStep.up);
        Debug.DrawRay(nextStepRay.origin, nextStepRay.direction, Color.green);

        //look to target ray
        lookToTarget.LookAt(target.pos);
        Ray rayToTarget = new Ray(lookToTarget.position, lookToTarget.forward);
        //Physics.Raycast(rayToTarget, out rayToTargetHitInfo);
        Debug.DrawRay(rayToTarget.origin, rayToTarget.direction * 100, Color.blue);

        Ray downRay = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(downRay, out downHitInfo, 1, groundMask))
        {
            if (distanceToTarget > 0.5f && Physics.Raycast(nextStepRay, out nextStepHitInfo, 1, groundMask))
            {
                transform.position = nextStepHitInfo.point;
            }
            transform.position = new Vector3(transform.position.x, downHitInfo.point.y + (transform.localScale.y / 2) + 0.1f, transform.position.z);


            if (distanceToTarget > 0.5f)
            {
                Vector3 xzTargetPos = target.pos;
                xzTargetPos.y = transform.position.y;
                Vector3 relativePos = xzTargetPos - transform.position;
                transform.rotation = Quaternion.LookRotation(relativePos);
                //rotates perpendicular to the ground
                mesh.rotation = Quaternion.Lerp(mesh.rotation, Quaternion.FromToRotation(Vector3.up, downHitInfo.normal) * Quaternion.LookRotation(relativePos), 8*Time.deltaTime);
            }

            print("Ant down hit: " + downHitInfo.collider.name);
        }
        Debug.DrawRay(downRay.origin, downRay.direction, Color.red);
    }
}