using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwing : MonoBehaviour
{
    public float intensity;
    public float smooth;
    bool _running;
    private Quaternion origin_rotation;
    float mouseX, mouseY;
    public void ReceiveInput(Vector2 mouseInput)
    {
        mouseX = mouseInput.x;
        mouseY = mouseInput.y;
    }
    public void ReceiveRunningBool(bool running)
    {
        _running = running;
    }

    private void Start()
    {
        origin_rotation = transform.localRotation;
    }

    private void Update()
    {
        UpdateSway();
        UpdateRunningSway();
    }


    /// TO DO
    private void UpdateRunningSway()
    {
    }

    private void UpdateSway()
    {

        //calculate target rotation
        Quaternion adjX = Quaternion.AngleAxis(-intensity * mouseX, Vector3.up);
        Quaternion adjY = Quaternion.AngleAxis(intensity * mouseY, Vector3.right);
        Quaternion target_rotation = origin_rotation * adjX * adjY;

        //rotate towards target rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, target_rotation, Time.deltaTime * smooth);
    }

}