using System;
using UnityEngine;

/* <summary>
/// Abstract base class for all status effects that can be applied to a character.
/// </summary> */
public abstract class StatusEffect : MonoBehaviour
{
    public string EffectName; // The name of the effect (e.g., "Poison", "Burn").
    public float Duration; // Duration of the effect in seconds.

    private float elapsedTime; // Tracks the elapsed time since the last effect application.

    /* <summary>
    /// Applies the effect to the character. Derived classes must implement the logic.
    /// </summary>
    /// <param name="character">The character to which the effect is applied.</param> */
    protected abstract void ApplyEffect(CharacterBase character);

    /* <summary>
    /// Removes the effect from the character. Derived classes can override if needed.
    /// </summary>
    /// <param name="character">The character from which the effect is removed.</param> */
    protected virtual void RemoveEffect(CharacterBase character)
    {
        Debug.Log($"{EffectName} effect has ended for {character.CharacterName}.");
    }

    /* <summary>
    /// Updates the effect over time. Call this in the character's Update loop or a manager class.
    /// </summary>
    /// <param name="character">The character affected by the status effect.</param> */
    public void UpdateEffect(CharacterBase character)
    {
        if (character == null)
            return;

        // Apply the effect periodically (e.g., once per second)
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1f)
        {
            elapsedTime = 0f;
            ApplyEffect(character);
        }

        // Decrease the duration
        Duration -= Time.deltaTime;

        // Remove the effect if the duration ends
        if (Duration <= 0)
        {
            RemoveEffect(character);
            character.ActiveStatusEffects.Remove(this);
        }
    }
}
