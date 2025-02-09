using App.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace App.Jobs.Consumer
{


    public class VideosConsumer : IVideosConsumer
    {
        private readonly IVideosService _appService;
        private readonly IModel _channel;
        private const string _queue = "fiap_video_queue";

        public VideosConsumer(
            IVideosService appService,
            IModel channel)
        {
            _appService = appService;
            _channel = channel;
        }

        public void Dispose()
        {
            _appService.Dispose();
            _channel.Dispose();
        }

        public Task<bool> ConsumirVideo()
        {
            _channel.QueueDeclare(queue: _queue,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                await _appService.ConsumerVideo(int.Parse(message));

                // Acknowledge the message to remove it from the queue
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: _queue,
                                   autoAck: false,
                                   consumer: consumer);

            return Task.FromResult(true);
        }
    }
}


