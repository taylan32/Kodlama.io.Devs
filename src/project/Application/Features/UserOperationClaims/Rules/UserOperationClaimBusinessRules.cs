using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserOperationClaims.Rules
{
    public class UserOperationClaimBusinessRules
    {
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOperationClaimRepository _operationClaimRepository;

        public UserOperationClaimBusinessRules(IUserOperationClaimRepository userOperationClaimRepository, IUserRepository userRepository, IOperationClaimRepository operationClaimRepository)
        {
            _userOperationClaimRepository = userOperationClaimRepository;
            _userRepository = userRepository;
            _operationClaimRepository = operationClaimRepository;
        }

        public async Task UserShouldExist(int id)
        {
            User? user = await _userRepository.GetAsync(u => u.Id == id);
            if(user == null)
            {
                throw new BusinessException("User does not exist.");
            }
        }

        public async Task OperationClaimShouldExist(int id)
        {
            OperationClaim? operationClaim = await  _operationClaimRepository.GetAsync(c => c.Id == id);
            if(operationClaim == null)
            {
                throw new BusinessException("Operation claim does not exist.");
            }
        }

        public async Task OperationClaimShouldNotAssignAgain(int userId, int claimId)
        {
            UserOperationClaim? userOperationClaim = await _userOperationClaimRepository.GetAsync(c => c.UserId == userId && c.OperationClaimId == claimId);
            if(userOperationClaim != null)
            {
                throw new BusinessException("The claim has already assigned to the user");
            }
        }

    }
}
