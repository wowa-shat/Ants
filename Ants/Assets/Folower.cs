using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folower : MonoBehaviour
{
    [SerializeField] Camera gameCamera;
    [SerializeField] Target target;
    [SerializeField] GameObject lookToTarget;

    RaycastHit downHitInfo;
    RaycastHit rayToTargetHitInfo;

    Vector3 currentPos;
    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        currentPos = transform.position;
        targetPos = target.targetMesh.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;
        targetPos = target.targetMesh.position;

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
                Vector3 xztargetPos = targetPos;
                xztargetPos.y = currentPos.y;
                transform.LookAt(xztargetPos);
            }

            print("Ant down hit: " + downHitInfo.collider.name);
        }
        Debug.DrawRay(downRay.origin, downRay.direction, Color.red);

        lookToTarget.transform.LookAt(targetPos);
        Ray rayToTarget = new Ray(lookToTarget.transform.position, lookToTarget.transform.forward);
        Debug.DrawRay(rayToTarget.origin, rayToTarget.direction * 100, Color.blue);

        //Ray forwardRay = new Ray(transform.position, transform.forward);
        //RaycastHit forwardHitInfo;
        //Physics.Raycast(forwardRay, out forwardHitInfo, 1);
        //Debug.DrawRay(forwardRay.origin, forwardRay.direction, Color.red);
    }
}