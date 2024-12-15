using AhlApp.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhlApp.Application.Features.Transactions.Commands
{
    public class TransferFundsCommand : IRequest<Response<Guid>>
    {
        public Guid SenderAccountId { get; set; }
        public Guid ReceiverAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
    }
}
