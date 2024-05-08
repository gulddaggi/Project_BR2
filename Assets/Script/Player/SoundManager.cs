using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] comboClips; 

    public int audioPoolSize = 5; 
    private List<AudioSource> audioPool = new List<AudioSource>();
    private Queue<AudioSource> availableAudioSources = new Queue<AudioSource>();

    [System.Serializable]
    public class FXSound
    {
        public AudioClip[] audioClip;
        public AudioClip specialAttackAudioClip;
    }
    public FXSound[] weaponSounds; // FXSound 클래스에서 무기 효과음 통합 관리

    private void Awake()
    {
        // 오브젝트 풀링
        for (int i = 0; i < audioPoolSize; i++)
        {
            GameObject obj = new GameObject("AudioSource");
            obj.transform.parent = transform;
            AudioSource audioSource = obj.AddComponent<AudioSource>();
            audioPool.Add(audioSource);
            availableAudioSources.Enqueue(audioSource);
        }
    }

    public void PlayWeaponSound(int ComboIndex)
    {
        if (GameManager_JS.Instance != null)
        {
            PlaySound(weaponSounds[GameManager_JS.Instance.PlayerWeaponCheck()].audioClip[ComboIndex]);
        }
        else
        {
            Debug.LogWarning("Cannot Find GameManager_JS!");
        }
    }

    public void SpecialAttackSound()
    {
        if (GameManager_JS.Instance != null)
        {
            PlaySound(weaponSounds[GameManager_JS.Instance.PlayerWeaponCheck()].specialAttackAudioClip);
        }
        else
        {
            Debug.LogWarning("Cannot Find GameManager_JS!");
        }
    }


    private void PlaySound(AudioClip clip)
    {
        if (availableAudioSources.Count > 0)
        {
            AudioSource audioSource = availableAudioSources.Dequeue();
            audioSource.clip = clip;
            audioSource.Play();
            StartCoroutine(ManagePool(audioSource));
        }
        else
        {
            Debug.LogWarning("No available audio source in the pool!");
        }
    }

    // 클립 끝나면 풀에 다시 집어넣음
    private System.Collections.IEnumerator ManagePool(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.clip = null;
        availableAudioSources.Enqueue(audioSource);
    }
}