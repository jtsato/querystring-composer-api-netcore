using System;

namespace Core.Commons;

public interface IGetDateTime
{
    DateTime Now();

    DateTime UtcNow();
}