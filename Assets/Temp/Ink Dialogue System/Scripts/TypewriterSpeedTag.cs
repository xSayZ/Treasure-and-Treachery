using UnityEngine;

[RequireComponent(typeof(TypewriterEffect))]
public class TypewriterSpeedTag : MonoBehaviour
{
    [Tooltip("Name of the Ink tag pair key which should control the typewriter speed.")]
    public string tagName = "speed";
    
    private float originalSpeed;
    private TypewriterEffect typewriter;

    private void Start()
    {
        typewriter = GetComponent<TypewriterEffect>();
        originalSpeed = typewriter.charactersPerSecond;
    }

    private void OnEnable()
    {
        DialogueSystem.OnLineTagPair += SetSpeed;
        TypewriterEffect.OnTypewriterStop += ResetSpeed;
    }

    private void OnDisable()
    {
        DialogueSystem.OnLineTagPair -= SetSpeed;
        TypewriterEffect.OnTypewriterStop -= ResetSpeed;
    }

    private void SetSpeed(string key, string speed)
    {
        if (key != tagName)
            return;
        originalSpeed = typewriter.charactersPerSecond;
        if(speed.Contains('.'))
            typewriter.charactersPerSecond = originalSpeed * float.Parse(speed);
        else
            typewriter.charactersPerSecond = float.Parse(speed);
    }

    private void ResetSpeed()
    {
        typewriter.charactersPerSecond = originalSpeed;
    }
}
