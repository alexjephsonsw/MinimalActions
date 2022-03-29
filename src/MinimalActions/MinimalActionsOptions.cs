namespace MinimalActions
{
    public class MinimalActionsOptions
    {
        public InstanceCreationType InstanceCreationType { get; set; }
    }

    public enum InstanceCreationType
    {
        InstancePerMethod,
        InstancePerType
    }
}