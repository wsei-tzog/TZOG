using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public struct Sound
    {
        public SoundType type;
        public List<AudioClip> clips;
    }

    public Sound[] sounds;

    private Dictionary<SoundType, List<AudioClip>> soundLists;

    private void Awake()
    {
        soundLists = new Dictionary<SoundType, List<AudioClip>>();
        foreach (Sound sound in sounds)
        {
            soundLists.Add(sound.type, sound.clips);
        }
    }

    public List<AudioClip> GetSoundList(SoundType type)
    {
        if (soundLists.TryGetValue(type, out List<AudioClip> soundList))
        {
            return soundList;
        }
        else
        {
            return null;
        }
    }

    public void PlaySound(AudioSource audioSource, SoundType soundType)
    {
        audioSource.enabled = true;

        List<AudioClip> soundList = GetSoundList(soundType);

        if (soundList != null)
        {
            AudioClip clip = soundList[Random.Range(0, soundList.Count)];
            audioSource.clip = clip;

            audioSource.pitch = Random.Range(0.85f, 1.3f);
            audioSource.volume = Random.Range(0.8f, 1);
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Sound type not found: " + soundType);
        }

    }
}

public enum SoundType
{
    Hit,
    BarrelHit,
    DoorsOpen,
    DoorsClosed,
    GateOpen,
    Interact,
    Glass,
    Metal,
    Run,
    Walk,
    SawPlayer,
    MeeleAttack,
    TookDamage,
    Die,
    Stun

}
