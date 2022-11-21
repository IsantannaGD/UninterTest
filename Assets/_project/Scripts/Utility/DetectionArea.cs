using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DetectionArea : MonoBehaviour
{
    public delegate void DetectionEvents(bool status, IPlayer player);
    public DetectionEvents onDetectionTriggerStart;
    public DetectionEvents onDetectionCollisionStart;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IPlayer player))
        {
            onDetectionTriggerStart?.Invoke(true, player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IPlayer player))
        {
            onDetectionTriggerStart?.Invoke(false, null);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IPlayer player))
        {
            onDetectionCollisionStart?.Invoke(true, player);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.TryGetComponent(out IPlayer player))
        {
            onDetectionCollisionStart?.Invoke(false, null);
        }
    }
}
