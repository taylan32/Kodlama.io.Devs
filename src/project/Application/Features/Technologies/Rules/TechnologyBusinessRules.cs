using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Technologies.Rules
{
    public class TechnologyBusinessRules
    {
        private readonly ITechnologyRepository _technologyRepository;

        public TechnologyBusinessRules(ITechnologyRepository technologyRepository)
        {
            _technologyRepository = technologyRepository;
        }

        public async Task TechnologyNameCanNotBeDuplicatedWhenInserted(string name)
        {
           IPaginate<Technology> result =  await _technologyRepository.GetListAsync(t => t.Name == name);
            if (result.Items.Any()) throw new BusinessException("Technology name exists.");
        }

        public async Task TechnologyShouldExistWhenRequested(int id)
        {
            Technology result = await _technologyRepository.GetAsync(t => t.Id == id);
            if (result == null) throw new BusinessException("Requested technology does not exist.");
        }

        public async Task TechnologyNameCanNotBeDuplicatedWhenUpdated(string name)
        {
            IPaginate<Technology> result = await _technologyRepository.GetListAsync(t => t.Name == name);
            if (result.Items.Any()) throw new BusinessException("Technology name exists.");
        }

    }
}
