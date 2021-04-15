[System.Serializable] public class DialogueConditions
{
    public DialogueConditions(ConditionalDialogueManager.ConditionNames conditionName, bool conditionStatus)
    {
        name = conditionName;
        state = conditionStatus;
        ConditionalDialogueManager.Instance.conditions.Add(this);
    }

    public ConditionalDialogueManager.ConditionNames name;
    public bool state;

    public bool State
    {
        get => state;
        set => state = value;
    }

    public void ConditionMet()
    {
        this.state = true;
    }

    public string GetNameAsString()
    {
        return name.ToString();
    }
}
