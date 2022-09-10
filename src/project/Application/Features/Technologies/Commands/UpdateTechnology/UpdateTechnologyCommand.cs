using Application.Features.Technologies.Dtos;
using Application.Features.Technologies.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Technologies.Commands.UpdateTechnology
{
    public class UpdateTechnologyCommand : IRequest<UpdatedTechnologyDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class UpdateTechnologyCommandHandler : IRequestHandler<UpdateTechnologyCommand, UpdatedTechnologyDto>
        {
            private readonly ITechnologyRepository _tecnologyRepository;
            private readonly IMapper _mapper;
            private readonly TechnologyBusinessRules _technologyBusinessRules;

            public UpdateTechnologyCommandHandler(ITechnologyRepository tecnologyRepository, IMapper mapper, TechnologyBusinessRules technologyBusinessRules)
            {
                _tecnologyRepository = tecnologyRepository;
                _mapper = mapper;
                _technologyBusinessRules = technologyBusinessRules;
            }

            public async Task<UpdatedTechnologyDto> Handle(UpdateTechnologyCommand request, CancellationToken cancellationToken)
            {
                await _technologyBusinessRules.TechnologyShouldExistWhenRequested(request.Id);
                await _technologyBusinessRules.TechnologyNameCanNotBeDuplicatedWhenUpdated(request.Name);

                Technology? technology = await _tecnologyRepository.GetAsync(t => t.Id == request.Id);
                _mapper.Map(request, technology);
                Technology updatedTechnology = await _tecnologyRepository.UpdateAsync(technology);
                UpdatedTechnologyDto updatedTechnologyDto = _mapper.Map<UpdatedTechnologyDto>(updatedTechnology);

                return updatedTechnologyDto;
            }
        }

    }
}
