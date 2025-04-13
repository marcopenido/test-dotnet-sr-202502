using ApplicantTracking.Application.UseCase.Candidate;
using ApplicantTracking.Exceptions;

using CommonTestUtilities.Requests;

using FluentAssertions;

using Xunit;

namespace Validators.Test.Candidate;

public class CandidateValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestCandidateJsonBuilder.Build();

        var result = new CandidateValidator().Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("         ")]
    [InlineData("")]
    public void Error_Name_Empty(string? name)
    {
        var request = RequestCandidateJsonBuilder.Build();
        request.Name = name!;

        var result = new CandidateValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.NAME_EMPTY);
    }

    [Fact]
    public void Error_Name_Too_Long()
    {
        var request = RequestCandidateJsonBuilder.Build();
        request.Name += new string('a', 80);

        var result = new CandidateValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.NAME_MAX_LENGTH);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("         ")]
    [InlineData("")]
    public void Error_Surname_Empty(string? surname)
    {
        var request = RequestCandidateJsonBuilder.Build();
        request.Surname = surname!;

        var result = new CandidateValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.SURNAME_EMPTY);
    }

    [Fact]
    public void Error_Surname_Too_Long()
    {
        var request = RequestCandidateJsonBuilder.Build();
        request.Surname += new string('a', 150);

        var result = new CandidateValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.SURNAME_MAX_LENGTH);
    }

    [Fact]
    public void Error_Birthdate_Empty()
    {
        var request = RequestCandidateJsonBuilder.Build();
        request.Birthdate = default;

        var result = new CandidateValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.BIRTHDATE_EMPTY);
    }

    [Fact]
    public void Error_Birthdate_Invalid_Future()
    {
        var request = RequestCandidateJsonBuilder.Build();
        request.Birthdate = DateTime.UtcNow.AddDays(1);

        var result = new CandidateValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.BIRTHDATE_INVALID);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("         ")]
    [InlineData("")]
    public void Error_Email_Empty(string? email)
    {
        var request = RequestCandidateJsonBuilder.Build();
        request.Email = email!;

        var result = new CandidateValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RequestCandidateJsonBuilder.Build();
        request.Email = "invalidEmail";

        var result = new CandidateValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.EMAIL_INVALID);
    }

    [Fact]
    public void Error_Email_Too_Long()
    {
        var request = RequestCandidateJsonBuilder.Build();
        request.Email = new string('a', 251) + "@test.com";

        var result = new CandidateValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.EMAIL_MAX_LENGTH);
    }
}
