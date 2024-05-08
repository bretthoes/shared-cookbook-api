using AutoFixture.Kernel;
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
        fixture.Customize(new AspNetCustomization());
        fixture.Customize(new ObjectValidatorCustomization());
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        return fixture;
    }
}

public class ObjectValidatorCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<CookbooksController>(c => c.Do(x => x.ObjectValidator = fixture.Create<IObjectModelValidator>()));
    }
}

public class AspNetCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new ControllerBasePropertyOmitter());
    }
}

public class ControllerBasePropertyOmitter : Omitter
{
    public ControllerBasePropertyOmitter()
        : base(new OrRequestSpecification(GetPropertySpecifications())) { }

    private static IEnumerable<IRequestSpecification> GetPropertySpecifications()
    {
        return typeof(ControllerBase).GetProperties().Where(x => x.CanWrite)
            .Select(x => new PropertySpecification(x.PropertyType, x.Name));
    }
}