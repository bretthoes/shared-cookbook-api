using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SharedCookbook.Api.Controllers;
using SharedCookbook.Api.Data.Dtos;
using SharedCookbook.Api.Data.Entities;
using SharedCookbook.Api.Repositories.Interfaces;
using SharedCookbook.Api.UnitTests.Attributes;

namespace SharedCookbook.Api.UnitTests.ControllerTests.CookbooksControllerTests;

public class PartiallyUpdateCookbookTests
{
    [Theory, AutoMoqData]
    public void WhenPatchDocIsNull_ThenBadRequestExpected(CookbooksController sut)
    {
        var actual = sut.PartiallyUpdateCookbook(It.IsAny<int>(), It.IsAny<JsonPatchDocument<CookbookDto>>());

        actual.Result.Should().BeOfType<BadRequestResult>();
    }

    [Theory, AutoMoqData]
    public void WhenPatchDocIsEmpty_ThenBadRequestExpected(CookbooksController sut)
    {
        var emptyPatchDoc = new JsonPatchDocument<CookbookDto>();

        var actual = sut.PartiallyUpdateCookbook(It.IsAny<int>(), emptyPatchDoc);

        actual.Result.Should().BeOfType<BadRequestResult>();
    }

    [Theory, AutoMoqData]
    public void WhenExistingCookbookNotFound_ThenNotFoundExpected(
        [Frozen] Mock<ICookbookRepository> repositoryMock,
        CookbooksController sut)
    {
        repositoryMock
            .Setup(m => m.GetSingle(It.IsAny<int>()))
            .Returns<CookbookDto>(null!);

        var actual = sut.PartiallyUpdateCookbook(It.IsAny<int>(), GetValidPatchDoc());

        actual.Result.Should().BeOfType<NotFoundResult>();
    }

    [Theory, AutoMoqData]
    public void WhenPatchOperationInvalid_ThenBadRequestExpected(CookbooksController sut)
    {
        var actual = sut.PartiallyUpdateCookbook(It.IsAny<int>(), GetInvalidPatchDoc());

        actual.Result.Should().BeOfType<BadRequestResult>();
    }

    [Theory, AutoMoqData]
    public void WhenPatchOperationMissingValue_ThenBadRequestExpected(CookbooksController sut)
    {
        var patchDoc = new JsonPatchDocument<CookbookDto>();
        patchDoc.Operations.Add(new Operation<CookbookDto>()
        {
            op = OperationType.Replace.ToString(),
        });

        var actual = sut.PartiallyUpdateCookbook(It.IsAny<int>(), patchDoc);

        actual.Result.Should().BeOfType<BadRequestResult>();
    }

    [Theory, AutoMoqData]
    public void WhenUpdateReturnsNull_ThenInternalServerErrorExpected(
        [Frozen] Mock<ICookbookRepository> repositoryMock,
        CookbooksController sut)
    {
        repositoryMock
            .Setup(m => m.Update(It.IsAny<Cookbook>()))
            .Returns<CookbookDto>(null!);

        var actual = sut.PartiallyUpdateCookbook(It.IsAny<int>(), GetValidPatchDoc());

        actual.Result.As<StatusCodeResult>().StatusCode
            .Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Theory, AutoMoqData]
    public void WhenSaveReturnsFalse_ThenInternalServerErrorExpected(
        [Frozen] Mock<ICookbookRepository> repositoryMock,
        CookbooksController sut)
    {
        repositoryMock
            .Setup(m => m.Save())
            .Returns(false);

        var actual = sut.PartiallyUpdateCookbook(It.IsAny<int>(), GetValidPatchDoc());

        actual.Result.As<StatusCodeResult>().StatusCode
            .Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Theory, AutoMoqData]
    public void WhenRequestIsValid_ThenOkResultExpected(CookbooksController sut)
    {
        var actual = sut.PartiallyUpdateCookbook(It.IsAny<int>(), GetValidPatchDoc());

        actual.Result.Should().BeOfType<OkObjectResult>();
    }

    private static JsonPatchDocument<CookbookDto> GetValidPatchDoc()
    {
        var patchDoc = new JsonPatchDocument<CookbookDto>();
        patchDoc.Operations.Add(new Operation<CookbookDto>()
        {
            op = OperationType.Replace.ToString(),
            path = "/imagePath",
            value = string.Empty,
        });
        return patchDoc;
    }

    private static JsonPatchDocument<CookbookDto> GetInvalidPatchDoc()
    {
        var patchDoc = new JsonPatchDocument<CookbookDto>();
        patchDoc.Operations.Add(new Operation<CookbookDto>()
        {
            op = OperationType.Invalid.ToString(),
            path = "/imagePath",
            value = string.Empty,
        });
        return patchDoc;
    }
}
