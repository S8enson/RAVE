using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhylloTunnel : MonoBehaviour
{
    public Transform tunnel;
    public Audio audio;
    public float tunnelSpeed, cameraDistance;
    
    public void setDistance(float dist){

        cameraDistance = dist;
    }

    // Update is called once per frame
    void Update()
    {
        
        tunnel.position = new Vector3(tunnel.position.x, tunnel.position.y, tunnel.position.z + (audio.audioBandBuffer[1] * tunnelSpeed));

        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, tunnel.position.z + cameraDistance);
    }
}
