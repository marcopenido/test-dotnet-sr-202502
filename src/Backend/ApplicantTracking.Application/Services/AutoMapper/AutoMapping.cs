using System.Collections.Generic;

using ApplicantTracking.Communication.Requests;
using ApplicantTracking.Communication.Responses;
using ApplicantTracking.Domain.Entities;

using AutoMapper;

namespace ApplicantTracking.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestCandidateJson, Candidate>();
    }

    private void DomainToResponse()
    {
        CreateMap<Candidate, ResponseCandidateJson>();

        CreateMap<IEnumerable<Candidate>, ResponseCandidatesJson>()
          .ForMember(dest => dest.Candidates,
                     opt => opt.MapFrom(src => src));
    }
}
