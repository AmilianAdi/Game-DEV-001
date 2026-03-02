using UnityEngine;
using System.Collections;

public class CamRotate : MonoBehaviour
{
    public float rotationDuration = 0.2f;
    private bool isRotating = false;
    public void RotateRight()
    {
        if (!isRotating)
            StartCoroutine(RotateBy(90f));
    }    public void RotateLeft()
    {
        if (!isRotating)
            StartCoroutine(RotateBy(-90f));
    }
    IEnumerator RotateBy(float angle)
    {
        isRotating = true;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, angle, 0);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / rotationDuration;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }
        transform.rotation = endRot;
        isRotating = false;
    }
}
