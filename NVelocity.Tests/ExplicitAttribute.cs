namespace NVelocity.Tests
{
    using System.Diagnostics;
    using Xunit;

    public class ExplicitAttribute : FactAttribute
    {
        public ExplicitAttribute()
        {
            if (!Debugger.IsAttached)
            {
                Skip = "Only running in interactive mode.";
            }
        }
    }
}
