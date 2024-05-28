﻿using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;
using Domain.Enums;

namespace Application.Roles.Commands.Create;

public sealed record CreateRoleCommand(string Name, List<Permissions> Permissions) : ICommand<Result>;
