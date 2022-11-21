using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookViewY : MonoBehaviour
{
    private void Update()
    {
        RotateViewY();
    }
    private void RotateViewY()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 localEulerAngles = transform.localEulerAngles;
        localEulerAngles.x += mouseY * -1;

        transform.localEulerAngles = localEulerAngles;
    }
}
