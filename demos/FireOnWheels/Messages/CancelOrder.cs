﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Messages
{
    public class CancelOrder
    {
        public Guid OrderId { get; set; }
    }
}