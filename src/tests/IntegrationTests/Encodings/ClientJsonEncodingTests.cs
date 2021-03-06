﻿using System.Threading.Tasks;
using FluentAssertions;
using MyNatsClient;
using MyNatsClient.Encodings.Json;
using MyNatsClient.Extensions;
using Xunit;

namespace IntegrationTests.Encodings
{
    public class ClientJsonEncodingTests : ClientIntegrationTests
    {
        private const string Subject = nameof(ClientJsonEncodingTests);

        private NatsClient _client;

        public ClientJsonEncodingTests()
        {
            _client = new NatsClient(ConnectionInfo);
            _client.Connect();
        }

        protected override void OnAfterEachTest()
        {
            _client?.Disconnect();
            _client?.Dispose();
            _client = null;
        }

        [Fact]
        public void Should_be_able_to_publish_and_consume_JSON_payloads_synchronously()
        {
            var orgItem = EncodingTestItem.Create();
            EncodingTestItem decodedItem = null;

            _client.Sub(Subject, stream => stream.Subscribe(msg =>
            {
                decodedItem = msg.FromJson<EncodingTestItem>();
                ReleaseOne();
            }));

            _client.PubAsJson(Subject, orgItem);
            WaitOne();

            orgItem.Should().BeEquivalentTo(decodedItem);
        }

        [Fact]
        public async Task Should_be_able_to_publish_and_consume_JSON_payloads_asynchronously()
        {
            var orgItem = EncodingTestItem.Create();
            EncodingTestItem decodedItem = null;

            await _client.SubAsync(Subject, stream => stream.Subscribe(msg =>
            {
                decodedItem = msg.FromJson<EncodingTestItem>();
                ReleaseOne();
            }));

            await _client.PubAsJsonAsync(Subject, orgItem);
            WaitOne();

            orgItem.Should().BeEquivalentTo(decodedItem);
        }
    }
}