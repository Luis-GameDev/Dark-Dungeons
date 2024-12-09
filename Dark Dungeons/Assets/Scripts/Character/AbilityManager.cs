using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{   
    public List<Ability> ActiveAbilities = new List<Ability>();
    [SerializeField] private CharacterBase cb;
    [SerializeField] private GameObject cooldownOverlay;
    [SerializeField] private GameObject abilityBar;

    void Start()
    {
        // get selected abilities from JSON and instanciate the ability into the ability list

        int abilityAmount = 0;
        foreach(var ability in ActiveAbilities) {
            //Instantiate(abilityButton, abilityBar);
            abilityAmount++;
        }
    }
    void FixedUpdate() {
        foreach(var ability in ActiveAbilities) {
            if(ability.cooldownLeft > 0) {
                ability.cooldownLeft -= Time.deltaTime;
            } else {
                ability.cooldownLeft = 0;
            }
        }
    }

    private void AbilityUse(Ability ability) {
        if(ability.manaCost > cb.Mana) {
            ShowInsufficientMana();
        } else if(ability.cooldownLeft <= 0) {
            ability.cooldownLeft = ability.cooldown;
            ability.Execute(cb);
        }
    }

    private void ShowInsufficientMana() {

    }
}
