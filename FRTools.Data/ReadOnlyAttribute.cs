using System.ComponentModel;

namespace FRTools.Data
{
    public class ReadOnlyAttribute : DescriptionAttribute
    {
        public ReadOnlyAttribute(string description) : base(description)
        {
        }
    }
}
