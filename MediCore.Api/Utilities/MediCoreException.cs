using System;

namespace MediCore.Api.Utilities;

public class MediCoreException:Exception
{
    public MediCoreException(string errMsg):base(errMsg){}
}
