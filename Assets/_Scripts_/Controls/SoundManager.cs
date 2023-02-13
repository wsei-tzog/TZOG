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
    Metal

}
