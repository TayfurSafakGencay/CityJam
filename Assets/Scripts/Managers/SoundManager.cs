using UnityEngine;

namespace Managers
{
  public class SoundManager : MonoBehaviour
  {
    public static SoundManager Instance;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }
    
    public AudioSource MusicAudioSource;
    
    public void PlayMusic(AudioClip clip)
    {
      MusicAudioSource.clip = clip;
      MusicAudioSource.Play();
    }

    public AudioSource EffectAudioSource;

    public void PlayEffect(AudioClip clip)
    {
      EffectAudioSource.PlayOneShot(clip);
    }

    [Header("Effects")]
    public AudioClip CollectSound;
  }
}