using System;

[Serializable]
public class TargetedAttempts
{
    public TargetRelationship targetRelationship = TargetRelationship.Hostile;
    public Attempt[] attempts;
}