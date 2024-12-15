﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhlApp.Application.DTOs.TransactionDTOs
{
    public class TransactionDto
    {
        public Guid TransactionId { get; set; }
        public Guid? SenderAccountId { get; set; }
        public Guid? ReceiverAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}