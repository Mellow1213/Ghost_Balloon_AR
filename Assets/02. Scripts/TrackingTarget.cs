using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackingTarget : MonoBehaviour
{
    private ObserverBehaviour track;

    bool isTracked;

    // Start is called before the first frame update
    void Start()
    {
        track = GetComponent<ObserverBehaviour>();
        if (track)
        {
            track.OnTargetStatusChanged += OnObserverStatusChanged;
            OnObserverStatusChanged(track, track.TargetStatus);
        }
    }

    void OnObserverStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        if (targetStatus.Status == Status.TRACKED)
        {
            isTracked = true;
        }
        else if (targetStatus.Status == Status.NO_POSE)
        {
            isTracked = false;
        }
    }

    public bool getIsTracked() { return isTracked; }
}
