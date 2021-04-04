using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    [HideInInspector] public AudioSource audioSource;
    [SerializeField] AudioClip bonus, bubble, button, coin, gun, heartbeat, lose, win;
    [SerializeField] AudioMixerGroup background, gameplay, master;
    public AudioMixerSnapshot standard, slow;
    Vector2 pitch;
    bool vibrate = false;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        pitch = new Vector2(0.9f, 1.1f);
    }
    private void FixedUpdate()
    {
        if(vibrate && SaveManager.Instance.SavedValues.Vibration)
            Handheld.Vibrate();
    }
    public void Vibrate(float time = 0.3f)
    {
        StartCoroutine(EnumVibrate(time));
    }
    IEnumerator EnumVibrate(float time)
    {
        vibrate = true;
        yield return new WaitForSecondsRealtime(time);
        vibrate = false;
    }
    void Play(AudioClip audioClip, Vector2 pitchRange, float volume)
    {
        audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        audioSource.PlayOneShot(audioClip, volume);
    }
    void SwitchMixer(AudioMixerGroup mixerGroup, string name, float endValue, float duration = 0)
    {
        mixerGroup.audioMixer.GetFloat(name, out float value);
        DOTween.To((a) => mixerGroup.audioMixer.SetFloat(name, a), value, endValue, duration);
    }
    public void SetBackground(float endValue, float duration = 0)
    {
        SwitchMixer(background, "Background", endValue, duration);
    }
    public void SetGameplay(float endValue, float duration = 0)
    {
        SwitchMixer(gameplay, "Gameplay", endValue, duration);
    }
    public void SetMaster(float endValue, float duration = 0)
    {
        SwitchMixer(master, "Master", endValue, duration);
    }
    public void Bonus(float volume = 1)
    {
        Play(bonus, pitch, volume);
    }
    public void Bubble(float volume = 1)
    {
        Play(bubble, pitch, volume);
    }
    public void Button(float volume = 1)
    {
        Play(button, pitch, volume);
    }
    public void Coin(float volume = 1)
    {
        Play(coin, pitch, volume);
    }
    public void Gun(float volume = 1, float pitch = 1)
    {
        Play(gun, this.pitch * pitch, volume);
    }
    public void Heartbeat(float volume = 1, bool start = true)
    {
        if (start)
        {
           audioSource.clip = heartbeat;
            audioSource.loop = true;
            //AudioMixer.SetFloat("Background", -40f);
            audioSource.Play();
        }
        else
        {
            audioSource.loop = false;
            audioSource.Stop();

        }

    }
    public void Lose(float volume = 1)
    {
        Play(lose, pitch, volume);
    }
    public void Win(float volume = 1)
    {
        Play(win, pitch, volume);
    }
}
