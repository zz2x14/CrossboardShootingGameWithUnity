using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOverdrive : MonoBehaviour
{
    public static UnityAction On = delegate { };
    public static UnityAction Off = delegate { };

    [SerializeField] private GameObject normalPlayerEngineVFX;
    [SerializeField] private GameObject overdrivePlayerEngineVFX;
    [SerializeField] private GameObject overdriveLancuVFX;
    [SerializeField] private AudioData overdriveOnAudioData;
    [SerializeField] private AudioData overdriveOffAudioData;

    private void OnEnable()
    {
        On += IsOn;
        Off += IsOff;
    }

    private void OnDisable()
    {
        On -= IsOn;
        Off -= IsOff;
    }

    private void IsOn()
    {
        AudioManager.Instance.PlaySFX(overdriveOnAudioData);

        overdriveLancuVFX.SetActive(true);
        normalPlayerEngineVFX.SetActive(false);
        overdrivePlayerEngineVFX.SetActive(true);
    }

    private void IsOff()
    {
        AudioManager.Instance.PlaySFX(overdriveOffAudioData);

        overdrivePlayerEngineVFX.SetActive(false);
        normalPlayerEngineVFX.SetActive(true);
    }
}
