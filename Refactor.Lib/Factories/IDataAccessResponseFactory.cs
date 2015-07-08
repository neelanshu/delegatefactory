using System;
using Refactor.Lib.Model;

namespace Refactor.Lib.Factories
{
    public interface IDataAccessResponseFactory<out T> where T: class 
    {
        Func<int, T> GetDataAccessResponseByFailoverState(FailoverState failoverState);
    }

   
}