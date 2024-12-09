using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Ability", menuName = "Ability/SO_Ability")]
public class SO_Ability : ScriptableObject
{
    [SerializeField] public Image abilityIcon;
    [SerializeField] public Ability abilityScript;
    [SerializeField] public string abilityName;
}
