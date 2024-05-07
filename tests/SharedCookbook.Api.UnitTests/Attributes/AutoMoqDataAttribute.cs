using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SharedCookbook.Api.Controllers;

namespace SharedCookbook.Api.UnitTests.Attributes;

public class AutoMoqDataAttribute() : AutoDataAttribute(FixtureFactory)
{
    private static IFixture FixtureFactory()
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        return fixture;
    }
}