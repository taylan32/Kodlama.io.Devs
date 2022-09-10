using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OperationClaims.Rules
{
    public class OperationClaimBusinessRules
    {
        private readonly IOperationClaimRepository _operationClaimRepository;

        public OperationClaimBusinessRules(IOperationClaimRepository operationClaimRepository)
        {
            _operationClaimRepository = operationClaimRepository;
        }

        public async Task OperaionClaimNameCanNotDuplicateWhenInserted(string name)
        {
            IPaginate<OperationClaim> operationClaims = await _operationClaimRepository.GetListAsync(c => c.Name == name);
            if (operationClaims.Items.Any()) throw new BusinessException("Operaion claim name exists.");
        }

    }
}
