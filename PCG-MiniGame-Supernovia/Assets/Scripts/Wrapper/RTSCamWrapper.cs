using RTS_Cam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RTS_Camera))]
public class RTSCamWrapper : MonoBehaviour
{
    RTS_Camera hookedCam;
    // Start is called before the first frame update
    void Start()
    {
        hookedCam = GetComponent<RTS_Camera>();
        SetCamXRotation(45f);
    }

    private void SetCamXRotation(float angle)
    {
        var t = hookedCam.transform;
        t.localEulerAngles = new Vector3(angle, t.localEulerAngles.y, t.localEulerAngles.z);
    }
}
