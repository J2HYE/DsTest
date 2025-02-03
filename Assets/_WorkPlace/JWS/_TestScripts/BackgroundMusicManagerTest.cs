using UnityEngine;
using System.Collections;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioSource audioSource1; // 첫 번째 AudioSource
    public AudioSource audioSource2; // 두 번째 AudioSource
    private AudioSource currentSource; // 현재 재생 중인 AudioSource
    private AudioSource nextSource; // 전환될 AudioSource

    public float transitionDuration = 2f;

    private void Start()
    {
        currentSource = audioSource1;
        nextSource = audioSource2;
        currentSource.Play();
    }

    public void ChangeBackgroundMusic(AudioClip newClip)
    {
        if (!currentSource.isPlaying) return;
        nextSource.clip = newClip;
        nextSource.Play();
        StartCoroutine(TransitionMusic());
    }

    private IEnumerator TransitionMusic()
    {
        float time = 0f;
        while (time < transitionDuration)
        {
            time += Time.deltaTime;
            float progress = time / transitionDuration;
            currentSource.volume = Mathf.Lerp(1f, 0f, progress);
            nextSource.volume = Mathf.Lerp(0f, 1f, progress);

            yield return null;
        }
        currentSource.Stop();
        currentSource.volume = 1f;
        AudioSource temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;
    }
}
