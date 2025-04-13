using System.Threading.Tasks;
using ApplicantTracking.Api.Controllers.Base;
using ApplicantTracking.Application.UseCase.Candidate.Create;
using ApplicantTracking.Application.UseCase.Candidate.Delete;
using ApplicantTracking.Application.UseCase.Candidate.GetAll;
using ApplicantTracking.Application.UseCase.Candidate.GetById;
using ApplicantTracking.Application.UseCase.Candidate.Update;
using ApplicantTracking.Communication.Requests;
using ApplicantTracking.Communication.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantTracking.Api.Controllers
{
    public sealed class CandidatesController : ApplicantTrackingController
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponseCandidatesJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> List([FromServices] IGetAllCadidateUseCase useCase)
        {
            var response = await useCase.Execute();

            if (response.Candidates?.Count > 0)
                return Ok(response);

            return NoContent();
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(ResponseCandidateJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int id, [FromServices] IGetCadidateByIdUseCase useCase)
        {
            var candidate = await useCase.Execute(id);

            return Ok(candidate);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseCandidateJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] RequestCandidateJson request, [FromServices] ICreateCandidateUseCase useCase)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }

        [HttpPut]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] RequestCandidateJson request, [FromServices] IUpdateCandidateUseCase useCase)
        {
            await useCase.Execute(id, request);

            return NoContent();
        }

        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id, [FromServices] IDeleteCandidateUseCase useCase)
        {
            await useCase.Execute(id);

            return NoContent();
        }
    }
}
