using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using TweetAPI.Services.Interfaces;
using TweetAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TweetAPI.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public SubscriberService(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public void GetDeletedFromQueue()
        {

            }
    }
}
