// ------------------------------------------------------------------------
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

namespace Dapr.Actors.Communication;

using System.Threading;

internal static class ActorLogicalCallContext
{
    private static readonly AsyncLocal<string> fabActAsyncLocal = new AsyncLocal<string>();

    public static bool IsPresent()
    {
        return (fabActAsyncLocal.Value != null);
    }

    public static bool TryGet(out string callContextValue)
    {
        callContextValue = fabActAsyncLocal.Value;
        return (callContextValue != null);
    }

    public static void Set(string callContextValue)
    {
        fabActAsyncLocal.Value = callContextValue;
    }

    public static void Clear()
    {
        fabActAsyncLocal.Value = null;
    }
}