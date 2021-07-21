using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecorder : MonoBehaviour
{
    private struct CameraSample
    {
        public float timestamp;
        public Vector3 position;
        public Quaternion rotation;
    }

    private class CameraRecording
    {
        public string name;
        public float startTime;
        public List<CameraSample> samples = new List<CameraSample>();
    }

    private CameraRecording currentRecording;
    private bool recording;

    void Update()
    {
        if (recording && currentRecording != null)
        {
            currentRecording.samples.Add(new CameraSample
            {
                timestamp = Time.time,
                position = transform.position,
                rotation = transform.rotation
            });
        }
    }

    public void SaveRecording()
    {
        // JSON.NET (Newtonsoft)
        // Serialize the currentRecording variable to a JSON string
        // Save the string to a file with name $"{DateTime.Now}.rec",
    }

    public void OnStartRecording()
    {
        currentRecording = new CameraRecording
        {
            startTime = Time.time
        };
        
        recording = true;
    }
    
    public void OnStopRecording()
    {
        recording = false;
    }
}
