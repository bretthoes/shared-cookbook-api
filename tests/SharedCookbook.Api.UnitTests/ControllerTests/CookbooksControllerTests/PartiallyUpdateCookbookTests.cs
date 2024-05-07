using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SharedCookbook.Api.Controllers;
using SharedCookbook.Api.Data.Entities;
using SharedCookbook.Api.Repositories.Interfaces;
using SharedCookbook.Api.UnitTests.Attributes;

namespace SharedCookbook.Api.UnitTests.ControllerTests.CookbooksControllerTests;

public class PartiallyUpdateCookbookTests
{
    [Theory, AutoMoqData]
    public void WhenPatchDocIsNull_ThenBadRequestExpected([NoAutoProperties] CookbooksController sut)
    {
        var actual = sut.PartiallyUpdateCookbook(It.IsAny<int>(), null!);

        actual.Result.Should().BeOfType<BadRequestResult>();
    }

    [Theory, AutoMoqData]
    public void WhenPatchDocIsEmpty_ThenBadRequestExpected([NoAutoProperties] CookbooksController sut)
    {
        var emptyPatchDoc = new JsonPatchDocument<Cookbook>();

        var actual = sut.PartiallyUpdateCookbook(It.IsAny<int>(), emptyPatchDoc);

        actual.Result.Should().BeOfType<BadRequestResult>();
    }

    [Theory, AutoMoqData]
    public void WhenExistingCookbookNotFound_ThenNotFoundExpected(
        [Frozen] Mock<ICookbookRepository> repositoryMock,
        [NoAutoProperties] CookbooksController sut)
    {
        var patchDoc = new JsonPatchDocument<Cookbook>();
        patchDoc.Operations.Add(new Operation<Cookbook>());
        repositoryMock
            .Setup(m => m.GetSingle(It.IsAny<int>()))
            .Returns<Cookbook>(null!);

        var actual = sut.PartiallyUpdateCookbook(It.IsAny<int>(), patchDoc);

        actual.Result.Should().BeOfType<NotFoundResult>();
    }

    [Theory, AutoMoqData]
    public void WhenPatchOperationInvalid_ThenBadRequestExpected([NoAutoProperties] CookbooksController sut)
    {
        var patchDoc = new JsonPatchDocument<Cookbook>();
        patchDoc.Operations.Add(new Operation<Cookbook>() 
        {
            op = OperationType.Invalid.ToString(),
        });

        var actual = sut.PartiallyUpdateCookbook(It.IsAny<int>(), patchDoc);

        actual.Result.Should().BeOfType<BadRequestResult>();
    }

    [Theory, AutoMoqData]
    public void WhenPatchOperationMissingValue_ThenBadRequestExpected([NoAutoProperties] CookbooksController sut)
    {
        var patchDoc = new JsonPatchDocument<Cookbook>();
        patchDoc.Operations.Add(new Operation<Cookbook>()
        {
            op = OperationType.Replace.ToString(),
        });

        var actual = sut.PartiallyUpdateCookbook(It.IsAny<int>(), patchDoc);

        actual.Result.Should().BeOfType<BadRequestResult>();
    }

    [Theory, AutoMoqData]
    public void WhenUpdateReturnsNull_ThenInternalServerErrorExpected(
        [Frozen] Mock<IObjectModelValidator> objectValidatorMock,
        [Frozen] Mock<ICookbookRepository> repositoryMock,
        [NoAutoProperties] CookbooksController sut)
    {
        sut.ObjectValidator = objectValidatorMock.Object;
        var patchDoc = new JsonPatchDocument<Cookbook>();
        patchDoc.Operations.Add(new Operation<Cookbook>()
        {
            op = OperationType.Replace.ToString(),
            path = "/imagePath",
            value = string.Empty,
        });
        repositoryMock
            .Setup(m => m.Update(It.IsAny<Cookbook>()))
            .Returns<Cookbook>(null!);

        var actual = sut.PartiallyUpdateCookbook(It.IsAny<int>(), patchDoc);

        actual.Result.As<StatusCodeResult>().StatusCode
            .Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Theory, AutoMoqData]
    public void WhenSaveReturnsFalse_ThenInternalServerErrorExpected(
        [Frozen] Mock<IObjectModelValidator> objectValidatorMock,
        [Frozen] Mock<ICookbookRepository> repositoryMock,
        [NoAutoProperties] CookbooksController sut)
    {
        sut.ObjectValidator = objectValidatorMock.Object;
        var patchDoc = new JsonPatchDocument<Cookbook>();
        patchDoc.Operations.Add(new Operation<Cookbook>()
        {
            op = OperationType.Replace.ToString(),
            path = "/imagePath",
            value = string.Empty,
        });
        repositoryMock
            .Setup(m => m.Save())
            .Returns(false);

        var actual = sut.PartiallyUpdateCookbook(It.IsAny<int>(), patchDoc);

        actual.Result.As<StatusCodeResult>().StatusCode
            .Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Theory, AutoMoqData]
    public void WhenRequestIsValid_ThenOkResultExpected(
        [Frozen] Mock<IObjectModelValidator> objectValidatorMock,
        [NoAutoProperties] CookbooksController sut)
    {
        sut.ObjectValidator = objectValidatorMock.Object;
        var patchDoc = new JsonPatchDocument<Cookbook>();
        patchDoc.Operations.Add(new Operation<Cookbook>()
        {
            op = OperationType.Replace.ToString(),
            path = "/imagePath",
            value = string.Empty,
        });

        var actual = sut.PartiallyUpdateCookbook(It.IsAny<int>(), patchDoc);

        actual.Result.Should().BeOfType<OkObjectResult>();
    }
}
