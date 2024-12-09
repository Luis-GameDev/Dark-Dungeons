using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ability_Rejuvenate : Ability
{

    public override void Execute(CharacterBase characterBase) {
        cooldown = 10f;        //characterBase.ActiveStatusEffects.Add();
    }
}
