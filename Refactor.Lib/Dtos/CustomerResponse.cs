using Refactor.Lib.Model;

namespace Refactor.Lib.Dtos
{
    public class CustomerResponse 
    {
        public bool IsArchived { get; set; }

        public Customer Customer { get; set; }
    }
}