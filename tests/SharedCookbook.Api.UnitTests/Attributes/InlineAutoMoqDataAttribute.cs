namespace SharedCookbook.Api.UnitTests.Attributes;

public class InlineAutoMoqDataAttribute(params object[] parameters)
    : InlineAutoDataAttribute(new AutoMoqDataAttribute(), parameters) { }
