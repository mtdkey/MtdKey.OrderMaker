using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Core.Approval
{
    public interface IApprovalService
    {
        Task<bool> StartApprovalChainAsync(Guid docId);

    }
}
