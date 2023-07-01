using System;

namespace TGSJuice
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class JuiceLabelAttribute : Attribute
    {
        public string Label { get; private set; }
        public string Icon { get; private set; }

        public JuiceLabelAttribute(string label, string icon)
        {
            Label = label;
            Icon = icon;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class JuiceDescriptionAttribute : Attribute
    {
        public string Description { get; private set; }

        public JuiceDescriptionAttribute(string discription)
        {
            Description = discription;
        }
    }
}