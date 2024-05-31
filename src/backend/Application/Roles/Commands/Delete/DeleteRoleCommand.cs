﻿using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Roles.Commands.Delete;

public sealed record DeleteRoleCommand(Guid Id) : ICommand<Result>;