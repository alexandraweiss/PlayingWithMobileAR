using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyMovement : MonoBehaviour
{
    [SerializeField]
    protected float changeTimer = 5f;
    [SerializeField]
    protected float currentSpeed = 0.2f;

    protected Vector3 currentDir = Vector3.one;
    protected bool lerpingFwd = false;
    protected Quaternion targetRotation = Quaternion.identity;
    protected Quaternion startRotation = Quaternion.identity;
    protected float angleToTarget = 0f;
    protected const float turnDuration = 4f;
    protected float turnStep = 1f;
    protected float turnDiff = 0f;

    protected float lastChange = 0f;

    protected RaycastHit raycastResult;
    protected float rayLength = 0.4f;
    protected Vector3 targetPosition = Vector3.zero;
    protected bool targetReached = true;


    private void Awake()
    {
        currentSpeed = Random.Range(0.15f, 0.25f);
        lastChange = - changeTimer;
    }

    private void Update()
    {
        //CheckForCollisions();
        SetDirection();
        Move();
    }

    private void SetDirection()
    {
        if (lerpingFwd && targetReached)
        {
            ChangeDirection();
        } 
        else if (lerpingFwd && !targetReached)
        {
            TurnTowardTarget();
        }
        else if (Time.time >= lastChange + changeTimer)
        {
            SetRandomDirection();
        }
    }

    protected void SetRandomDirection()
    {
        if (lerpingFwd)
        {
            return;
        }
        if (!targetReached)
        {
            return;
        }

        float currentYaw = transform.rotation.eulerAngles.y;
        float targetTilt = Random.Range(-10f, 10f);
        float targetYaw = Random.Range(currentYaw - 5f, currentYaw + 5f);


        targetRotation = Quaternion.Euler(targetTilt, targetYaw, 0f);
        startRotation = transform.rotation;
        lerpingFwd = true;
        lastChange = Time.time;
        turnDiff = Time.deltaTime;
        turnStep = Time.deltaTime / turnDuration;
    }

    protected void ChangeDirection()
    {
        if (turnDiff < turnDuration)
        {
            turnStep = turnDiff / turnDuration;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, turnDiff / turnDuration);
            turnDiff += Time.deltaTime;
        }
        else 
        {
            transform.rotation = targetRotation;
            lerpingFwd = false;
        }
    }

    protected void TurnTowardTarget()
    {
        if (!lerpingFwd)
        {
            return;
        }
        if (turnDiff < turnDuration)
        {
            if ((Time.frameCount & 25) == 0)
            {
                Quaternion tempRot = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);
                targetRotation = Quaternion.RotateTowards(transform.rotation, tempRot, 180f);

                startRotation = transform.rotation;
            }
            turnStep = turnDiff / turnDuration;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, turnDiff / turnDuration);
            turnDiff += Time.deltaTime;
        }
        else
        {
            transform.rotation = targetRotation;
            lerpingFwd = false;
        }
        
    }

    protected void Move()
    {
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }
    
    private void CheckForCollisions()
    {
        if (Physics.Raycast(transform.position + transform.forward * 0.05f, transform.forward, out raycastResult, rayLength))
        {
            AvoidWalls();
        }
    }

    protected void AvoidWalls()
    {
        if (lerpingFwd)
        {
            return;
        }
        lastChange = Time.time + 3f * turnDuration;
        targetRotation = Quaternion.LookRotation(-transform.forward, Vector3.up);
        startRotation = transform.rotation;
        lerpingFwd = true;
        turnStep = Time.deltaTime / (2f * turnDuration);
        turnDiff = Time.deltaTime;
    }


    public void SetTarget(Transform target)
    {
        if (Vector3.Distance(target.position, transform.position) < 0.01f)
        {
            OnTargetReached();
            return;
        }
        Vector3 nextPosition = transform.position + (transform.forward * currentSpeed * Time.deltaTime);
        targetPosition = target.position;
        Quaternion tempRot = Quaternion.LookRotation(targetPosition - nextPosition, Vector3.up);
        targetRotation = Quaternion.RotateTowards(transform.rotation, tempRot, 180f);

        startRotation = transform.rotation;
        lerpingFwd = true;
        lastChange = Time.time;
        turnStep = Time.deltaTime /  turnDuration;
        targetReached = false;
        turnDiff = Time.deltaTime;
    }

    public void OnTargetReached()
    {
        targetRotation = Quaternion.FromToRotation(transform.forward, targetPosition - transform.forward);
        Vector3 axis = new Vector3(0f, 0f, 0f);
        targetRotation.ToAngleAxis(out angleToTarget, out axis);
        transform.rotation = targetRotation;

        // Reset values
        targetPosition = Vector3.positiveInfinity;
        startRotation = Quaternion.identity;
        targetReached = true;
        lerpingFwd = false;
        turnDiff = Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        if (targetPosition != Vector3.zero)
        {
            Debug.DrawLine(transform.position, targetPosition, Color.red);
        }
    }
}
