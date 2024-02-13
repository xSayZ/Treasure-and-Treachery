using System;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(AudioSource))]
public class DialogueVocal : MonoBehaviour
{
    private AudioSource audioSource;
    [Tooltip("Randomize between these audio clips.")]
    public AudioClip[] clips;
    [Tooltip("The audio clips' pitch will be randomized between Pitch Min and Pitch Max.")]
    public float pitchMin = 1.0f;
    [Tooltip("The audio clips' pitch will be randomized between Pitch Min and Pitch Max.")]
    public float pitchMax = 4.0f;
    
    private void Awake() 
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void Play(char character)
    {
        Random random = new Random(char.ToLower(character) + gameObject.name.GetHashCode());
        float pitch = (float)(random.NextDouble() * (pitchMax - pitchMin) + pitchMin);
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clips[random.Next(clips.Length)]);
    }
}
