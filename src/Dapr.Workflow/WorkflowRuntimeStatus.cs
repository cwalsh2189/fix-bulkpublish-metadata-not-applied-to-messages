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

namespace Dapr.Workflow;

/// <summary>
/// Enum describing the runtime status of a workflow.
/// </summary>
public enum WorkflowRuntimeStatus
{
    /// <summary>
    /// The status of the workflow is unknown.
    /// </summary>
    Unknown = -1,

    /// <summary>
    /// The workflow started running.
    /// </summary>
    Running,

    /// <summary>
    /// The workflow completed normally.
    /// </summary>
    Completed,

    /// <summary>
    /// The workflow completed with an unhandled exception.
    /// </summary>
    Failed,

    /// <summary>
    /// The workflow was abruptly terminated via a management API call.
    /// </summary>
    Terminated,

    /// <summary>
    /// The workflow was scheduled but hasn't started running.
    /// </summary>
    Pending,

    /// <summary>
    /// The workflow was suspended.
    /// </summary>
    Suspended,
}