using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Range(1,10)]
    public float smoothFactor;
    public Vector3 minValues, maxValues;

    private void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 boundPosition = new Vector3(
            Mathf.Max(targetPosition.x, minValues.x),
            Mathf.Max(targetPosition.y, minValues.y),
            Mathf.Clamp(targetPosition.z, minValues.z, maxValues.z)
        );

        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor*Time.fixedDeltaTime);
        smoothPosition.z = transform.position.z;
        transform.position = smoothPosition;
    }
}