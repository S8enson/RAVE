using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FadeAudioSource {

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            Settings.instance.playerBTN.SetActive(false);
            Settings.instance.pauseBTN.SetActive(false);
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            // yield return new WaitForSeconds(2);
            yield return null;
            // yield return new WaitForSeconds(1f);
        }
        audioSource.Pause();
        Settings.instance.playerBTN.SetActive(true);
        audioSource.volume = 1f;//start;
        yield return new WaitForSeconds(2f);
    }
}