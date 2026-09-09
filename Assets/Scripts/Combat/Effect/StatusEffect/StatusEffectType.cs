public enum StatusEffectType
{
    None,

    // TODO: Move User Specific Buffs to instead be in its own enum(?) Since it's a StanceChange.
    Valenian,
    Mirrored,
    Painted,

    // Buffs
    Lifesteal,
    Rally,
    Barrier,
    Taunt,
    Freecast,

    // Debuffs
    Sunder,
    Starburn,
    Helltouched,
    Stunned,
    Frosted,
    Frozen,
    Drowsy,
    Sleep,
}