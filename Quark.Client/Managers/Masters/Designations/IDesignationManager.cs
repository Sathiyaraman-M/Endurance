﻿using Quark.Core.Features.Designations.Commands;
using Quark.Core.Responses;
using Quark.Shared.Wrapper;

namespace Quark.Client.Managers.Masters.Designations;

public interface IDesignationManager
{
    Task<IResult<List<DesignationResponse>>> GetAllAsync();

    Task<IResult<int>> SaveAsync(AddEditDesignationCommand request);

    Task<IResult<int>> DeleteAsync(int id);
}