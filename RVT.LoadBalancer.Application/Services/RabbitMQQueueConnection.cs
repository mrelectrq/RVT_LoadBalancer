using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RVT.LoadBalancer.Application.Services
{
    public class RabbitMQQueueConnection : IQueueConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        bool _disposed;


        public RabbitMQQueueConnection(IConnectionFactory connectionFactory)
        {
            
            _connectionFactory = connectionFactory;
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public void CreateConsumerChannel()
        {
            if(!IsConnected)
            {
                TryConnect();
            }

            // need to init queue handler 
        }

        public IModel CreateModel()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            if(_disposed)
            {
                return;
            }
            Dispose();
        }

        public bool TryConnect()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch(BrokerUnreachableException e)
            {
                Thread.Sleep(5000);
                Console.WriteLine("Error while connecting to the RabbitMQ server" + e.Message +"\r\n Trying to connect again...");
                _connection = _connectionFactory.CreateConnection();
            }
            if(IsConnected)
            {
                _connection.ConnectionBlocked += OnConnectionBlocked;
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
            }

            Console.WriteLine("Connected to RabbitMQ server: " + _connection.Endpoint.HostName);

            return true;
        }


        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs eventArgs)
        {
            if (_disposed) return;
            Console.WriteLine(eventArgs.Reason);
            TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs eventArgs)
        {
            if (_disposed) return;
            Console.WriteLine(eventArgs.Detail);
            TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (_disposed) return;
            Console.WriteLine(e.Cause);
            TryConnect();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            try
            {
                _connection.Dispose();
            }
           catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
