using UnityEngine;

public class VisualFollowCameraRotation : MonoBehaviour
{
    public Transform cameraPivot;
    public float yOffset = 0f;

    private void Update()
    {
        if (cameraPivot == null) return;

        Vector3 euler = transform.rotation.eulerAngles;
        euler.y = cameraPivot.rotation.eulerAngles.y + yOffset;

        transform.rotation = Quaternion.Euler(euler);
    }
}
