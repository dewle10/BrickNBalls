using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private AudioClip _shootClip;
    [SerializeField] private AudioClip _clickClip;
    private float _SoundTimer;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    public void PlayHitSound()
    {
        if (Time.time >= _SoundTimer)
        {
            _source.pitch = Random.Range(0.97f, 1.07f);
            _source.PlayOneShot(_hitClip);
            _SoundTimer = Time.time + 0.05f;
        }
    }
    public void PlayShootSound()
    {
        _source.PlayOneShot(_shootClip);
    }
    public void PlayClickSound()
    {
        _source.PlayOneShot(_clickClip);
    }
}