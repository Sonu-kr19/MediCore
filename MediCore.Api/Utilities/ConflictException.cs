using System;

namespace MediCore.Api.Utilities;

public class ConflictException:Exception
{
    public ConflictException(string errMsg) : base(errMsg)
    {
        
    }
}
