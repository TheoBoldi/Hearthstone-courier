using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource soundEffectSource;
    
    [Header("Sound Effects")]
    [SerializeField] private AudioClip interactionSound;
    [SerializeField] private AudioClip questAcceptSound;
    [SerializeField] private AudioClip deliveryCompleteSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        // Ensure we have an AudioSource
        if (soundEffectSource == null)
        {
            soundEffectSource = gameObject.AddComponent<AudioSource>();
        }

        // Initialize audio settings
        InitializeAudioSource();
    }

    private void InitializeAudioSource()
    {
        if (soundEffectSource != null)
        {
            soundEffectSource.volume = 1.0f;
            soundEffectSource.mute = false;
            soundEffectSource.playOnAwake = false;
            soundEffectSource.loop = false;
            soundEffectSource.spatialBlend = 0f; // 2D sound
        }
    }

    // Public methods that can be called from other scripts
    public void PlayInteractionSound()
    {
        PlaySound(interactionSound, "Interaction");
    }
    
    public void PlayQuestAcceptSound()
    {
        PlaySound(questAcceptSound, "Quest Accept");
    }
    
    public void PlayDeliveryCompleteSound()
    {
        PlaySound(deliveryCompleteSound, "Delivery Complete");
    }

    private void PlaySound(AudioClip clip, string soundName)
    {
        if (clip != null && soundEffectSource != null)
        {
            soundEffectSource.PlayOneShot(clip);
            Debug.Log($"AudioManager: Playing {soundName} sound");
        }
        else if (clip == null)
        {
            Debug.LogWarning($"AudioManager: {soundName} sound clip is not assigned!");
        }
    }
}