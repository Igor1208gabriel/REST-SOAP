using System.ServiceModel;

[ServiceContract]
public interface IService
{
    [OperationContract]
    string Authenticate(string username, string password);
}
