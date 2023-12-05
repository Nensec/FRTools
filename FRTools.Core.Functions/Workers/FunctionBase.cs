namespace FRTools.Core.Functions.Workers
{
    public abstract class FunctionBase
    {
#if DEBUG
        protected const bool DEBUG = true;
#else
        protected const bool DEBUG = false;
#endif
    }
}
