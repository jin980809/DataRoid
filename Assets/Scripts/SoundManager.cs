using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//������ Ÿ���̴�. ���带 �ߴ��� �ĺ��ϱ����� ����Ѵ�.
public enum SoundType
{
    BGM,
    EFFECT,
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<SoundManager>();
            }

            return m_instance;
        }
    }
    private static SoundManager m_instance;

    /// <summary>
    /// ����� �ͼ�, ������� Ÿ�Ժ��� ���带 ������ �� �ֵ��� �Ѵ�.
    /// </summary>
    [SerializeField] private AudioMixer mAudioMixer;

    //�ɼǿ��� ������ ���� ������ǰ� ȿ�� ������ �ҷ��̴�. ȿ���� BGM�� ������ ��� �Ҹ��� �ҷ��� ����Ѵ�.
    private float mCurrentBGMVolume, mCurrentEffectVolume;

    /// <summary>
    /// Ŭ������ ��� ��ųʸ�
    /// </summary>
    private Dictionary<string, AudioClip> mClipsDictionary;

    /// <summary>
    /// ������ �̸� �ε��Ͽ� ����� Ŭ����
    /// </summary>
    [SerializeField] private AudioClip[] mPreloadClips;

    private List<TemporarySoundPlayer> mInstantiatedSounds;

    private void Start()
    {
        mClipsDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in mPreloadClips)
        {
            mClipsDictionary.Add(clip.name, clip);
        }

        mInstantiatedSounds = new List<TemporarySoundPlayer>();


        PlaySound2D("Background", 0, true, SoundType.BGM);
    }

    /// <summary>
    /// ������� �̸��� ������� ã�´�.
    /// </summary>
    /// <param name="clipName">������� �̸�(���� �̸� ����)</param>
    /// <returns></returns>
    private AudioClip GetClip(string clipName)
    {
        AudioClip clip = mClipsDictionary[clipName];

        if (clip == null) { Debug.LogError(clipName + "�� �������� �ʽ��ϴ�."); }

        return clip;
    }

    /// <summary>
    /// ���带 ����� ��, ���� ���·� ����Ȱ�쿡�� ���߿� �����ϱ����� ����Ʈ�� �����Ѵ�.
    /// </summary>
    /// <param name="soundPlayer"></param>
    private void AddToList(TemporarySoundPlayer soundPlayer)
    {
        mInstantiatedSounds.Add(soundPlayer);
    }

    /// <summary>
    /// ���� ���� �� ����Ʈ�� �ִ� ������Ʈ�� �̸����� ã�� �����Ѵ�.
    /// </summary>
    /// <param name="clipName"></param>
    public void StopLoopSound(string clipName)
    {
        foreach (TemporarySoundPlayer audioPlayer in mInstantiatedSounds)
        {
            if (audioPlayer.ClipName == clipName)
            {
                mInstantiatedSounds.Remove(audioPlayer);
                Destroy(audioPlayer.gameObject);
                return;
            }
        }

        Debug.LogWarning(clipName + "�� ã�� �� �����ϴ�.");
    }

    /// <summary>
    /// 2D ����� ����Ѵ�. �Ÿ��� ��� ���� ���� �Ҹ� ũ��� �鸰��.
    /// </summary>
    /// <param name="clipName">����� Ŭ�� �̸�</param>
    /// <param name="type">����� ����(BGM, EFFECT ��.)</param>
    public void PlaySound2D(string clipName, float delay = 0f, bool isLoop = false, SoundType type = SoundType.EFFECT)
    {
        GameObject obj = new GameObject("TemporarySoundPlayer 2D");
        TemporarySoundPlayer soundPlayer = obj.AddComponent<TemporarySoundPlayer>();

        //������ ����ϴ°�� ���带 �����Ѵ�.
        if (isLoop) { AddToList(soundPlayer); }

        soundPlayer.InitSound2D(GetClip(clipName));
        soundPlayer.Play(mAudioMixer.FindMatchingGroups(type.ToString())[0], delay, isLoop);
    }

    /// <summary>
    /// 3D ����� ����Ѵ�.
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="audioTarget"></param>
    /// <param name="type"></param>
    /// <param name="attachToTarget"></param>
    /// <param name="minDistance"></param>
    /// <param name="maxDistance"></param>
    public void PlaySound3D(string clipName, Transform audioTarget, float delay = 0f, bool isLoop = false, SoundType type = SoundType.EFFECT, bool attachToTarget = true, float minDistance = 0.0f, float maxDistance = 15.0f)
    {
        GameObject obj = new GameObject("TemporarySoundPlayer 3D");
        obj.transform.localPosition = audioTarget.transform.position;
        if (attachToTarget) { obj.transform.parent = audioTarget; }

        TemporarySoundPlayer soundPlayer = obj.AddComponent<TemporarySoundPlayer>();

        //������ ����ϴ°�� ���带 �����Ѵ�.
        if (isLoop) { AddToList(soundPlayer); }

        soundPlayer.InitSound3D(GetClip(clipName), minDistance, maxDistance);
        soundPlayer.Play(mAudioMixer.FindMatchingGroups(type.ToString())[0], delay, isLoop);
    }

    //���� �ε�� �� �ɼ� �Ŵ��������� ��� ���� �ҷ��� ����� �ɼ��� ũ��� �ʱ�ȭ��Ű�� �Լ�.
    public void InitVolumes(float bgm, float effect)
    {
        SetVolume(SoundType.BGM, bgm);
        SetVolume(SoundType.EFFECT, effect);
    }

    //�ɼ��� ������ �� �Ҹ��� �ҷ��� �����ϴ� �Լ�
    public void SetVolume(SoundType type, float value)
    {
        mAudioMixer.SetFloat(type.ToString(), value);
    }

    /// <summary>
    /// ������ ���带 �����ϱ����� ���� ���� ���� (included)
    /// </summary>
    /// <param name="from">�����ϴ� �ε��� ��ȣ</param>
    /// <param name="includedTo">������ �ε��� ��ȣ(����)</param>
    /// <param name="isStartZero">���ڸ��ϰ�� 0���� �����ϴ°�? ��)01</param>
    /// <returns></returns>
    public static string Range(int from, int includedTo, bool isStartZero = false)
    {
        if (includedTo > 100 && isStartZero) { Debug.LogWarning("0�� ������ ���ڸ��� �������� �ʽ��ϴ�."); }

        int value = UnityEngine.Random.Range(from, includedTo + 1);

        return value < 10 && isStartZero ? '0' + value.ToString() : value.ToString();
    }
}