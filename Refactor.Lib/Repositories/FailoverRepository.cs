using System.Collections.Generic;
using Refactor.Lib.Model;

namespace Refactor.Lib.Repositories
{
    public interface IFailoverRepository
    {
        List<FailoverEntry> GetFailOverEntries();
    }

    public class FailoverRepository : IFailoverRepository
    {
        public List<FailoverEntry> GetFailOverEntries()
        {
            // return all from fail entries from database
            return new List<FailoverEntry>();
        } 
    }
}