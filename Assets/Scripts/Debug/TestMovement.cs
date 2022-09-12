using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    [SerializeField]
    protected float changeTimer = 5f;
    [SerializeField]
    protected float currentSpeed = 1.8f;

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

    Transform target;


    private void Awake()
    {
        //currentSpeed = Random.Range(0.15f, 0.25f);
        lastChange = -changeTimer;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Time.time > 2f && targetPosition == Vector3.zero)
        {
            GameObject[] flowers = GameObject.FindGameObjectsWithTag("Flower");
            SetTarget(flowers[0].transform);
        }
#endif
        Move();
    }

    protected void TurnTowardTarget()
    {
        //if (!lerpingFwd)
        //{
        //    return;
        //}
        //Quaternion newTargetRotation = Quaternion.LookRotation(targetPosition, Vector3.up);
        //Vector3 axis = Vector3.zero;
        //newTargetRotation.ToAngleAxis(out angleToTarget, out axis);
        //float rotationSpeed = angleToTarget / turnDuration;
        //float rotationStep = rotationSpeed * Time.deltaTime;
        //Debug.Log(rotationStep);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, newTargetRotation, rotationStep);
    }

    protected void Move()
    {
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }

    public void SetTarget(Transform target)
    {
        Debug.LogWarning(string.Format("Set target to  {0}   {1}", target.name, Time.time));
        //targetPosition = target.position;
        Vector3 nextPos = transform.position + transform.forward * currentSpeed * Time.deltaTime;
        
        this.target = target;
        targetPosition = target.position - (target.forward * currentSpeed * Time.deltaTime);
        targetRotation = Quaternion.FromToRotation(nextPos - transform.position, targetPosition - nextPos);

        //Vector3 axis = new Vector3(0f, 0f, 0f);
        //targetRotation.ToAngleAxis(out angleToTarget, out axis);
        
        startRotation = transform.rotation;
        lerpingFwd = true;
        lastChange = Time.time;
        turnStep = Time.deltaTime / turnDuration;
        targetReached = false;
        turnDiff = Time.deltaTime;

        //TODO
        transform.rotation *= targetRotation;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, targetPosition, Color.red);
        if (target != null)
        {
            Debug.DrawLine(target.position, targetPosition, Color.green);
        }
    }
}
