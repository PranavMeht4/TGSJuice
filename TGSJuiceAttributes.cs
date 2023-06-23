using System;

namespace TGSJuice
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class JuiceLabelAttribute : Attribute
    {
        public string Label { get; private set; }

        public JuiceLabelAttribute(string label)
        {
            Label = label;
        }
    }
}