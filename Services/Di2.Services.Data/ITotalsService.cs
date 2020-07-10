using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Di2.Services.Data
{
    public interface ITotalsService
    {
        int GetStatus(int orderId);

        Task<int> ChangeOrderStatus(int orderId, int isCompleted);
    }
}
