using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace FRTools.Discord.Preconditions
{
    public class ExactValuePreconditionAttribute : ParameterPreconditionAttribute
    {
        public ExactValuePreconditionAttribute(object exactValue)
        {
            ExactValue = exactValue;
        }
        public object ExactValue { get; }

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, ParameterInfo parameter, object value, IServiceProvider services)
        {
            if (parameter.IsOptional && value == null)
                return PreconditionResult.FromSuccess();

            return value.Equals(ExactValue) ? PreconditionResult.FromSuccess() : PreconditionResult.FromError($"**{value}** must equal **{ExactValue}**");                
        }
    }
}
