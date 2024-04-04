using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShakeManager : MonoBehaviour
{
    public static ShakeManager Instance;

    public CinemachineVirtualCamera VCam;
    CinemachineBasicMultiChannelPerlin ShakeEffect;
    float ShakeTime;
    float StartInt;
    float TotalTime;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        VCam = GetComponent<CinemachineVirtualCamera>();
        ShakeEffect = VCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ShakeTime > 0)
        {
            ShakeTime -= Time.deltaTime;
        }
        else
        {
            ShakeEffect.m_AmplitudeGain = Mathf.Lerp(StartInt, 0,1-(ShakeTime/ TotalTime));
        }
    }
    public void Shake(int intensity ,float time)
    {
        ShakeEffect.m_AmplitudeGain = intensity;
        ShakeTime = time;
        StartInt = intensity;
        TotalTime = time;
    }
}
