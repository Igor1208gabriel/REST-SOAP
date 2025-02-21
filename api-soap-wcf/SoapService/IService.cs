using System.ServiceModel;
using System.Threading.Tasks;

[ServiceContract]
public interface IService
{
    [OperationContract]
    Task<string> Authenticate(string username, string password);
}
