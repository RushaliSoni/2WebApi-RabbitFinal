using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receiver_RabbitMq.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiverController : ControllerBase
    {
        //private object message;

        //private object message;
        


        [HttpGet]
        public IActionResult Index()
        {
            var message = " ";
            var factory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Rushalisoni",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                
                channel.BasicConsume(queue: "Rushalisoni",
                                     autoAck: true,
                                     consumer: consumer);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    message = Encoding.UTF8.GetString(body);
                   
                };
                channel.BasicConsume(queue: "Rushalisoni",
                                     autoAck: true,
                                     consumer: consumer);

                channel.QueuePurge("Rushalisoni");
                return Ok(message);
                

                
            }
        }
    }
}