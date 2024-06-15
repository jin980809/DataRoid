using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class TemporarySoundPlayer : MonoBehaviour
{
    private AudioSource mAudioSource;
    public string ClipName
    {
        get
        {
            return mAudioSource.clip.name;
        }
    }

    public void Awake()
    {
        mAudioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioMixerGroup audioMixer, float delay, bool isLoop)
    {
        mAudioSource.outputAudioMixerGroup = audioMixer;
        mAudioSource.loop = isLoop;
        mAudioSource.Play();

        if (!isLoop) { StartCoroutine(COR_DestroyWhenFinish(mAudioSource.clip.length)); }
    }

    public void InitSound2D(AudioClip clip)
    {
        mAudioSource.clip = clip;
    }

    public void InitSound3D(AudioClip clip, float minDistance, float maxDistance)
    {
        mAudioSource.clip = clip;
        mAudioSource.spatialBlend = 1.0f;
        mAudioSource.rolloffMode = AudioRolloffMode.Linear;
        mAudioSource.minDistance = minDistance;
        mAudioSource.maxDistance = maxDistance;

        AnimationCurve ac = mAudioSource.GetCustomCurve(AudioSourceCurveType.SpatialBlend);
        Keyframe[] keys = new Keyframe[1];

        for (int i = 0; i < keys.Length; ++i)
        {
            keys[i].value = 1f;
        }

        ac.keys = keys;
        mAudioSource.SetCustomCurve(AudioSourceCurveType.SpatialBlend, ac);
    }

    private IEnumerator COR_DestroyWhenFinish(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        Destroy(gameObject);
    }
}