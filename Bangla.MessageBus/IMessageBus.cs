using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangla.MessageBus
{
    public interface IMessageBus
    {
        // topic and queue name must be identical in azure service bus
        Task SendMessageAsync(object message, string topic_queue_name);

    }
}
