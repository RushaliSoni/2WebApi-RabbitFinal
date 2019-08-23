using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace RabbitMq_webApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SenderController : ControllerBase
    {
        private readonly ILogger _logger;

        public SenderController(ILogger<SenderController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Index()
        {
             var factory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localHost"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Rushalisoni",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                  arguments: null);
                //Console.WriteLine("Enter Your Message u want to send ");
                //string message = Console.ReadLine();
                var message = "Message From Sender";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                                     routingKey: "Rushalisoni",
                                     basicProperties: null,
                                     body: body);
                _logger.LogInformation(message);


                //Log.Logger = new LoggerConfiguration().WriteTo.File("TextFile.txt").CreateLogger();
                //TextWriter tw = new StreamWriter("TextFile.txt");
                //TextReader tr = new StreamReader("TextFile.txt");
                // tw.WriteLine(message);
                
                return Ok( message);
             }
            

        }

    }
}