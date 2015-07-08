using Refactor.Lib.Model;

namespace Refactor.Lib.Services
{
    public interface IFailoverService
    {
        FailoverState CurrentFailoverState{ get;}
    }
}