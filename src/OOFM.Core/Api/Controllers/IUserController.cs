namespace OOFM.Core.Api.Controllers;

public interface IUserController
{
    Task<string> AppendToken(string url, CancellationToken cancellationToken);
}
