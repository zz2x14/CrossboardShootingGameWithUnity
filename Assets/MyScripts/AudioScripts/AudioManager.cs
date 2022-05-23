using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersisentSingletonTool<AudioManager>
{
    [SerializeField] private AudioSource sFXPlayer;

    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    public void PlaySFX(AudioData audioData)
    {
        sFXPlayer.PlayOneShot(audioData.audioClip, audioData.audioVolume);//�ò��ŷ���ÿ�β���ʱ�Ḳ��֮ǰ��
    }

    public void PlayRandomSFX(AudioData audioData)
    {
        sFXPlayer.pitch = Random.Range(minPitch, maxPitch);
        PlaySFX(audioData);
    }

    public void PlayRandomSFX(AudioData[] audioDatas)
    {
        PlaySFX(audioDatas[Random.Range(0,audioDatas.Length)]);
    }
    
}
