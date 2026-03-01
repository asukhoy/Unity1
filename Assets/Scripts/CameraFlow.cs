using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float fixedX = 0f;
    public float fixedY = 2f;
    public float offsetZ = -5f;
    public float smoothSpeed = 5f;

    public bool lookAtTarget = false;

    private void LateUpdate()
    {
        if (target == null) return;

        float targetZ = target.position.z + offsetZ;

        Vector3 currentPos = transform.position;
        float newZ = Mathf.Lerp(currentPos.z, targetZ, smoothSpeed * Time.deltaTime);
        transform.position = new Vector3(fixedX, fixedY, newZ);

        if (lookAtTarget)
        {
            transform.LookAt(target);
        }
    }
}