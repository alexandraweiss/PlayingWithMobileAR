using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    [SerializeField]
    protected float changeTimer = 5f;
    [SerializeField]
    protected float currentSpeed = 0.2f;
    
    protected Vector3 currentDir = Vector3.one;
    protected bool lerpingFwd = false;
    protected Quaternion targetRotation = Quaternion.identity;
    protected float turnDuration = 2f;
    protected float turnStep = 1f;
    protected float turnInt = 0f;

    protected float lastChange = 0f;

    protected RaycastHit raycastResult;
    protected float rayLength = 0.4f;

    private void Start()
    {
        currentSpeed = Random.Range(0.15f, 0.25f);
    }

    private void Update()
    {
        if (Time.time >= lastChange + changeTimer) 
        {
            SetNewTargetDirection();
        }
        if (lerpingFwd) 
        {
            ChangeDirection();
        }

        Move();
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position + transform.forward * 0.05f, transform.forward, out raycastResult, rayLength))
        {
            //DebugOutput.instance.ShowMessage(Vector3.Dot(transform.forward, raycastResult.normal).ToString());
            AvoidCollision();
        }
    }

    public void OnSpawn() 
    {
        SetNewTargetDirection();
    }

    protected void SetNewTargetDirection() 
    {
        if (lerpingFwd)
        {
            return;
        }
        float currentYaw = transform.rotation.eulerAngles.y;
        float targetTilt = Random.Range(-20f, 20f);
        float targetYaw = Random.Range(currentYaw - 35f, currentYaw + 35f);

        targetRotation = Quaternion.Euler(targetTilt, targetYaw, 0f);
        
        lerpingFwd = true;
        lastChange = Time.time;
        turnStep = Time.deltaTime / turnDuration;
    }

    protected void AvoidCollision() 
    {
        if (lerpingFwd)
        {
            return;
        }
        lastChange = Time.time + 3f * turnDuration;
        targetRotation = Quaternion.LookRotation(- transform.forward, Vector3.up);
        lerpingFwd = true;
        turnStep = Time.deltaTime / (2f * turnDuration);
    }

    protected void ChangeDirection() 
    {
        if (Quaternion.Dot(transform.rotation, targetRotation) >= 0.99) 
        {
            lerpingFwd = false;
            turnInt = 0;
            return;
        }
        
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnInt);
        turnInt += turnStep;
    }

    protected void Move()
    {
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position + transform.forward * 0.05f, transform.forward * rayLength, Color.red);
    }
}
