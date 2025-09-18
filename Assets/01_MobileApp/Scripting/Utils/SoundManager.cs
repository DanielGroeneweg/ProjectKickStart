using UnityEngine;
using System.Collections;
using Unity.Burst.Intrinsics;

public class SoundManager : MonoBehaviour
{
    [Header("Lootboxes")]
    [SerializeField] private AudioClip lootbox;
    [SerializeField] private AudioClip common;
    [SerializeField] private AudioClip rare;
    [SerializeField] private AudioClip epic;
    [SerializeField] private AudioClip legendary;

    [Header("Buttons")]
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip pageSwitch;

    [Header("Music")]
    [SerializeField] private AudioClip music;

    [Header("Prefab")]
    [SerializeField] private AudioSource audioClipPrefab;

    [HideInInspector] public SoundManager instance;
    private IEnumerator PlaySound(AudioClip clip, bool looping)
    {
        AudioSource audio = Instantiate(audioClipPrefab);
        audio.clip = clip;
        audio.Play();

        if (looping)
        {
            audio.loop = true;
            yield break;
        }

        yield return new WaitForSeconds(clip.length);
        Destroy(audio.gameObject);
    }
    #region functions
    public void PlayLootboxSound()
    {
        StartCoroutine(PlaySound(lootbox, false));
    }
    public void PlayLootboxResultSound(Enums.Rarities rarity)
    {
        AudioClip clip = rarity switch
        {
            Enums.Rarities.Common => common,
            Enums.Rarities.Rare => rare,
            Enums.Rarities.Epic => epic,
            Enums.Rarities.Legendary => legendary,
            _ => common
        };
        StartCoroutine(PlaySound(clip, false));
    }
    public void PlayClickSound()
    {
        StartCoroutine(PlaySound(buttonClick, false));
    }
    public void PlayPageSound()
    {
        StartCoroutine(PlaySound(pageSwitch, false));
    }
    public void PlayMusic()
    {
        StartCoroutine(PlaySound(music, true));
    }
    #endregion

    #region Setup
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    #endregion
}