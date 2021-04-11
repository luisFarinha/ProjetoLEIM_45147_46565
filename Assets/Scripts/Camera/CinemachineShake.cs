using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{

    private CinemachineVirtualCamera cvc;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    void Awake()
    {
        cvc = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin cbmcp =
                cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cbmcp.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cbmcp =
            cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cbmcp.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }


}
