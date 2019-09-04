using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Model
{
    public class Order : IMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
