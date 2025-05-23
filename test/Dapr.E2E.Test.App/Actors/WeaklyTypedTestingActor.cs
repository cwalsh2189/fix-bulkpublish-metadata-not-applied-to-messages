﻿// ------------------------------------------------------------------------
// Copyright 2021 The Dapr Authors
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

using System.Threading.Tasks;
using Dapr.Actors.Runtime;

namespace Dapr.E2E.Test.Actors.WeaklyTypedTesting;

public class WeaklyTypedTestingActor : Actor, IWeaklyTypedTestingActor
{
    public WeaklyTypedTestingActor(ActorHost host)
        : base(host)
    {
    }

    public Task<ResponseBase> GetNullResponse()
    {
        return Task.FromResult<ResponseBase>(null);
    }

    public Task<ResponseBase> GetPolymorphicResponse()
    {
        var response = new DerivedResponse
        {
            BasePropeprty = "Base property value",
            DerivedProperty = "Derived property value"
        };

        return Task.FromResult<ResponseBase>(response);
    }

    public Task Ping()
    {
        return Task.CompletedTask;
    }
}