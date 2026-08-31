using System;
using System.Collections.Generic;
using UnityEngine;

// Temporary
public class CombatUIManager : MonoBehaviour
{
    public static CombatUIManager Instance { get; private set; }

    [SerializeField] private Battle battle;
    
    [SerializeField] private CharacterResourceBars attacker;
    [SerializeField] private CharacterResourceBars defender;

    private Dictionary<Combatant, CharacterResourceBars> playerResourceBars = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Register(battle.Attacker, attacker);
        Register(battle.Defender, defender);
    }

    public void Register(Combatant combatant, CharacterResourceBars characterResourceBars)
    {
        playerResourceBars[combatant] = characterResourceBars;

        InitializeCharacterResourceBars(characterResourceBars, combatant);
    }

    public void InitializeCharacterResourceBars(CharacterResourceBars characterResourceBars, Combatant combatant)
    {
        characterResourceBars.HPBar.SetMinMaxValues(0, combatant.Stats.MaxHP);
        characterResourceBars.MPBar.SetMinMaxValues(0, combatant.Stats.MaxMP);

        characterResourceBars.HPBar.SetValue(combatant.Stats.CurrentHP);
        characterResourceBars.MPBar.SetValue(combatant.Stats.CurrentMP);
    }

    public void UpdateCharacterResourceBars(Combatant combatant)
    {
        if (!playerResourceBars.TryGetValue(combatant, out CharacterResourceBars characterResourceBars))
            return;

        characterResourceBars.HPBar.SetValue(combatant.Stats.CurrentHP);
        characterResourceBars.MPBar.SetValue(combatant.Stats.CurrentMP);
    }
}

[Serializable]
public class CharacterResourceBars
{
    [SerializeField] private CombatResourceBar hpBar;
    [SerializeField] private CombatResourceBar mpBar;

    public CombatResourceBar HPBar => hpBar;
    public CombatResourceBar MPBar => mpBar;
}