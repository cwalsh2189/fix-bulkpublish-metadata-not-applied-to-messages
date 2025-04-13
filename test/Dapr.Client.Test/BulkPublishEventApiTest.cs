﻿// ------------------------------------------------------------------------
// Copyright 2023 The Dapr Authors
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ------------------------------------------------------------------------

namespace Dapr.Client.Test;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Autogenerated = Dapr.Client.Autogen.Grpc.v1;
using Shouldly;
using Grpc.Core;
using Moq;
using Xunit;

public class BulkPublishEventApiTest
{
    const string TestPubsubName = "testpubsubname";
    const string TestTopicName = "test";
    const string TestContentType = "application/json";
    static readonly List<string> bulkPublishData = new List<string>() { "hello", "world" };

    [Fact]
    public async Task BulkPublishEventAsync_CanPublishTopicWithEvents()
    {
        await using var client = TestClient.CreateForDaprClient();

        var request = await client.CaptureGrpcRequestAsync(async daprClient => 
            await daprClient.BulkPublishEventAsync(TestPubsubName, TestTopicName, bulkPublishData));

        request.Dismiss();

        var envelope = await request.GetRequestEnvelopeAsync<Autogenerated.BulkPublishRequest>();

        envelope.Entries.Count.ShouldBe(2);
        envelope.PubsubName.ShouldBe(TestPubsubName);
        envelope.Topic.ShouldBe(TestTopicName);
        envelope.Metadata.Count.ShouldBe(0);

        var firstEntry = envelope.Entries[0];

        firstEntry.EntryId.ShouldBe("0");
        firstEntry.ContentType.ShouldBe(TestContentType);
        firstEntry.Event.ToStringUtf8().ShouldBe(JsonSerializer.Serialize(bulkPublishData[0], client.InnerClient.JsonSerializerOptions));
        firstEntry.Metadata.ShouldBeEmpty();

        var secondEntry = envelope.Entries[1];

        secondEntry.EntryId.ShouldBe("1");
        secondEntry.ContentType.ShouldBe(TestContentType);
        secondEntry.Event.ToStringUtf8().ShouldBe(JsonSerializer.Serialize(bulkPublishData[1], client.InnerClient.JsonSerializerOptions));
        secondEntry.Metadata.ShouldBeEmpty();
            
        // Create Response & Respond
        var response = new Autogenerated.BulkPublishResponse
        {
            FailedEntries = { }
        };
        var bulkPublishResponse = await request.CompleteWithMessageAsync(response);
            
        // Get response and validate
        bulkPublishResponse.FailedEntries.Count.ShouldBe(0);
    }

    [Fact]
    public async Task BulkPublishEventAsync_CanPublishTopicWithEvents_WithMetadata()
    {
        await using var client = TestClient.CreateForDaprClient();

        var metadata = new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } };

        var request = await client.CaptureGrpcRequestAsync(async daprClient => 
            await daprClient.BulkPublishEventAsync(TestPubsubName, TestTopicName, bulkPublishData, 
                metadata));

        request.Dismiss();

        var envelope = await request.GetRequestEnvelopeAsync<Autogenerated.BulkPublishRequest>();

        envelope.Entries.Count.ShouldBe(2);
        envelope.PubsubName.ShouldBe(TestPubsubName);
        envelope.Topic.ShouldBe(TestTopicName);

        envelope.Metadata.Count.ShouldBe(2);
        envelope.Metadata.Keys.Contains("key1").ShouldBeTrue();
        envelope.Metadata.Keys.Contains("key2").ShouldBeTrue();
        envelope.Metadata["key1"].ShouldBe("value1");
        envelope.Metadata["key2"].ShouldBe("value2");

        var firstEntry = envelope.Entries[0];

        firstEntry.EntryId.ShouldBe("0");
        firstEntry.ContentType.ShouldBe(TestContentType);
        firstEntry.Event.ToStringUtf8().ShouldBe(JsonSerializer.Serialize(bulkPublishData[0], client.InnerClient.JsonSerializerOptions));
        firstEntry.Metadata.ShouldBeEmpty();

        var secondEntry = envelope.Entries[1];

        secondEntry.EntryId.ShouldBe("1");
        secondEntry.ContentType.ShouldBe(TestContentType);
        secondEntry.Event.ToStringUtf8().ShouldBe(JsonSerializer.Serialize(bulkPublishData[1], client.InnerClient.JsonSerializerOptions));
        secondEntry.Metadata.ShouldBeEmpty();
            
        // Create Response & Respond
        var response = new Autogenerated.BulkPublishResponse
        {
            FailedEntries = { }
        };
        var bulkPublishResponse = await request.CompleteWithMessageAsync(response);
            
        // Get response and validate
        bulkPublishResponse.FailedEntries.Count.ShouldBe(0);
    }

    [Fact]
    public async Task BulkPublishEventAsync_CanPublishTopicWithNoContent()
    {
        await using var client = TestClient.CreateForDaprClient();

        var request = await client.CaptureGrpcRequestAsync(async daprClient => 
            await daprClient.BulkPublishEventAsync(TestPubsubName, TestTopicName, bulkPublishData, 
                null));

        request.Dismiss();

        var envelope = await request.GetRequestEnvelopeAsync<Autogenerated.BulkPublishRequest>();
        envelope.Entries.Count.ShouldBe(2);
        envelope.PubsubName.ShouldBe(TestPubsubName);
        envelope.Topic.ShouldBe(TestTopicName);
        envelope.Metadata.Count.ShouldBe(0);

        var firstEntry = envelope.Entries[0];

        firstEntry.EntryId.ShouldBe("0");
        firstEntry.ContentType.ShouldBe(TestContentType);
        firstEntry.Event.ToStringUtf8().ShouldBe(JsonSerializer.Serialize(bulkPublishData[0], client.InnerClient.JsonSerializerOptions));
        firstEntry.Metadata.ShouldBeEmpty();

        var secondEntry = envelope.Entries[1];

        secondEntry.EntryId.ShouldBe("1");
        secondEntry.ContentType.ShouldBe(TestContentType);
        secondEntry.Event.ToStringUtf8().ShouldBe(JsonSerializer.Serialize(bulkPublishData[1], client.InnerClient.JsonSerializerOptions));
        secondEntry.Metadata.ShouldBeEmpty();
            
        // Create Response & Respond
        var response = new Autogenerated.BulkPublishResponse
        {
            FailedEntries = { }
        };
        var bulkPublishResponse = await request.CompleteWithMessageAsync(response);
            
        // Get response and validate
        bulkPublishResponse.FailedEntries.Count.ShouldBe(0);
    }

    [Fact]
    public async Task BulkPublishEventAsync_CanPublishTopicWithNoContent_WithMetadata()
    {
        await using var client = TestClient.CreateForDaprClient();

        var metadata = new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } };

        var request = await client.CaptureGrpcRequestAsync(async daprClient => 
            await daprClient.BulkPublishEventAsync(TestPubsubName, TestTopicName, bulkPublishData, 
                metadata));

        request.Dismiss();

        var envelope = await request.GetRequestEnvelopeAsync<Autogenerated.BulkPublishRequest>();
        envelope.Entries.Count.ShouldBe(2);
        envelope.PubsubName.ShouldBe(TestPubsubName);
        envelope.Topic.ShouldBe(TestTopicName);

        envelope.Metadata.Count.ShouldBe(2);
        envelope.Metadata.Keys.Contains("key1").ShouldBeTrue();
        envelope.Metadata.Keys.Contains("key2").ShouldBeTrue();
        envelope.Metadata["key1"].ShouldBe("value1");
        envelope.Metadata["key2"].ShouldBe("value2");

        var firstEntry = envelope.Entries[0];

        firstEntry.EntryId.ShouldBe("0");
        firstEntry.ContentType.ShouldBe(TestContentType);
        firstEntry.Event.ToStringUtf8().ShouldBe(JsonSerializer.Serialize(bulkPublishData[0], client.InnerClient.JsonSerializerOptions));
        firstEntry.Metadata.ShouldBeEmpty();

        var secondEntry = envelope.Entries[1];

        secondEntry.EntryId.ShouldBe("1");
        secondEntry.ContentType.ShouldBe(TestContentType);
        secondEntry.Event.ToStringUtf8().ShouldBe(JsonSerializer.Serialize(bulkPublishData[1], client.InnerClient.JsonSerializerOptions));
        secondEntry.Metadata.ShouldBeEmpty();
            
        // Create Response & Respond
        var response = new Autogenerated.BulkPublishResponse
        {
            FailedEntries = { }
        };
        var bulkPublishResponse = await request.CompleteWithMessageAsync(response);
            
        // Get response and validate
        bulkPublishResponse.FailedEntries.Count.ShouldBe(0);
    }

    [Fact]
    public async Task BulkPublishEventAsync_CanPublishTopicWithEventsObject()
    {
        await using var client = TestClient.CreateForDaprClient();

        var bulkPublishDataObject = new Widget() { Size = "Big", Color = "Green" };

        var request = await client.CaptureGrpcRequestAsync(async daprClient => 
            await daprClient.BulkPublishEventAsync(TestPubsubName, TestTopicName,
                new List<Widget> { bulkPublishDataObject }, null));

        request.Dismiss();

        var envelope = await request.GetRequestEnvelopeAsync<Autogenerated.BulkPublishRequest>();
        envelope.Entries.Count.ShouldBe(1);
        envelope.PubsubName.ShouldBe(TestPubsubName);
        envelope.Topic.ShouldBe(TestTopicName);
        envelope.Metadata.Count.ShouldBe(0);

        var firstEntry = envelope.Entries[0];

        firstEntry.EntryId.ShouldBe("0");
        firstEntry.ContentType.ShouldBe(TestContentType);
        firstEntry.Event.ToStringUtf8().ShouldBe(JsonSerializer.Serialize(bulkPublishDataObject, client.InnerClient.JsonSerializerOptions));
        firstEntry.Metadata.ShouldBeEmpty();
            
        // Create Response & Respond
        var response = new Autogenerated.BulkPublishResponse
        {
            FailedEntries = { }
        };
        var bulkPublishResponse = await request.CompleteWithMessageAsync(response);
            
        // Get response and validate
        bulkPublishResponse.FailedEntries.Count.ShouldBe(0);
    }
        
    [Fact]
    public async Task BulkPublishEventAsync_WithCancelledToken()
    {
        await using var client = TestClient.CreateForDaprClient();

        var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await client.InnerClient.BulkPublishEventAsync(TestPubsubName, TestTopicName, bulkPublishData,
                null, cancellationToken: cts.Token);
        });
    }
        
    [Fact]
    public async Task BulkPublishEventAsync_WrapsRpcException()
    {
        var client = new MockClient();

        var rpcStatus = new Status(StatusCode.Internal, "not gonna work");
        var rpcException = new RpcException(rpcStatus, new Metadata(), "not gonna work");

        // Setup the mock client to throw an Rpc Exception with the expected details info
        client.Mock
            .Setup(m => m.BulkPublishEventAlpha1Async(
                It.IsAny<Autogen.Grpc.v1.BulkPublishRequest>(), 
                It.IsAny<CallOptions>()))
            .Throws(rpcException);

        var ex = await Assert.ThrowsAsync<DaprException>(async () => 
        {
            await client.DaprClient.BulkPublishEventAsync(TestPubsubName, TestTopicName, 
                bulkPublishData);
        });
        Assert.Same(rpcException, ex.InnerException);
    }
        
    [Fact]
    public async Task BulkPublishEventAsync_FailureResponse()
    {
        await using var client = TestClient.CreateForDaprClient();
        
        var metadata = new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } };
        
        var request = await client.CaptureGrpcRequestAsync(async daprClient =>
        {
            return await daprClient.BulkPublishEventAsync(TestPubsubName, TestTopicName, 
                bulkPublishData, metadata);
        });
        
        request.Dismiss();
        
        // Create Response & Respond
        var response = new Autogenerated.BulkPublishResponse
        {
            FailedEntries =
            {
                new Autogenerated.BulkPublishResponseFailedEntry
                {
                    EntryId = "0",
                    Error = "Failed to publish",
                },
                new Autogenerated.BulkPublishResponseFailedEntry
                {
                    EntryId = "1",
                    Error = "Failed to publish",
                },
            }
        };
        var bulkPublishResponse = await request.CompleteWithMessageAsync(response);
        
        // Get response and validate
        bulkPublishResponse.FailedEntries[0].Entry.EntryId.ShouldBe("0");
        bulkPublishResponse.FailedEntries[0].ErrorMessage.ShouldBe("Failed to publish");
            
        bulkPublishResponse.FailedEntries[1].Entry.EntryId.ShouldBe("1");
        bulkPublishResponse.FailedEntries[1].ErrorMessage.ShouldBe("Failed to publish");
    }

    private class Widget
    {
        public string Size { get; set; }
        public string Color { get; set; }
    }
}