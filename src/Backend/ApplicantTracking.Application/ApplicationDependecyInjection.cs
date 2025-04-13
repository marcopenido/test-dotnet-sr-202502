using ApplicantTracking.Application.Services.AutoMapper;
using ApplicantTracking.Application.UseCase.Candidate.Create;
using ApplicantTracking.Application.UseCase.Candidate.Delete;
using ApplicantTracking.Application.UseCase.Candidate.GetAll;
using ApplicantTracking.Application.UseCase.Candidate.GetById;
using ApplicantTracking.Application.UseCase.Candidate.Update;

using AutoMapper;

using Microsoft.Extensions.DependencyInjection;

namespace ApplicantTracking.Application;

public static class ApplicationDependecyInjection
{
    public static void AddAutoMapper(IServiceCollection services)
    {
        services.AddScoped(option => new MapperConfiguration(autoMapperOption =>
        {
            autoMapperOption.AddProfile(new AutoMapping());
        }).CreateMapper());
    }

    public static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IGetAllCadidateUseCase, GetAllCadidateUseCase>();
        services.AddScoped<IGetCadidateByIdUseCase, GetCadidateByIdUseCase>();
        services.AddScoped<ICreateCandidateUseCase, CreateCandidateUseCase>();
        services.AddScoped<IUpdateCandidateUseCase, UpdateCandidateUseCase>();
        services.AddScoped<IDeleteCandidateUseCase, DeleteCandidateUseCase>();
    }
}
