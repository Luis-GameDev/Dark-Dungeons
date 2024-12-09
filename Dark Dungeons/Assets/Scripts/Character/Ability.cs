using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public abstract class Ability : MonoBehaviour
    {
        [Header("Values")]
        public float cooldown;
        public float manaCost;


        [Header("Data")]
        public float cooldownLeft;

        public abstract void Execute(CharacterBase character);
    }
