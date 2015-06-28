namespace OctoStyle.Console.Tests.Integration
{
    using TechTalk.SpecFlow;

    public class FeatureContextExtended : FeatureContext
    {
        private readonly FeatureContext currentContext;

        public FeatureContextExtended(FeatureContext context)
            : base(context.FeatureInfo, context.BindingCulture)
        {
            this.currentContext = context;
        }
    }
}
