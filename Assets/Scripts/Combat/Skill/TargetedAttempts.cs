using System;

[Serializable]
public class TargetedAttempts
{
    public TargetRelationshipType targetRelationship = TargetRelationshipType.Hostile;
    public Attempt[] attempts;
}