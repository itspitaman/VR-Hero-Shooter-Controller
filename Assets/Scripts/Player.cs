using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class Player : MonoBehaviour
{
    XROrigin xROrigin;
    public CapsuleCollider playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        xROrigin = GetComponent<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPosition = xROrigin.CameraInOriginSpacePos;
        playerCollider.height = Mathf.Clamp(cameraPosition.y, 1f, 2.5f);
        playerCollider.center = new Vector3(cameraPosition.x, cameraPosition.y / 2, cameraPosition.z);
    }
}
