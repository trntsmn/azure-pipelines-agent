// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Agent.Sdk;
using Agent.Plugins.PipelineArtifact;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.Content.Common.Tracing;

namespace Agent.Plugins
{
    internal class ArtifactProviderFactory
    {
        private readonly FileContainerProvider fileContainerProvider;
        private readonly PipelineArtifactProvider pipelineArtifactProvider;
        private readonly FileShareProvider fileShareProvider;

        public ArtifactProviderFactory(AgentTaskPluginExecutionContext context, VssConnection connection, IAppTraceSource tracer)
        {
            pipelineArtifactProvider = new PipelineArtifactProvider(context, connection, tracer);
            fileContainerProvider = new FileContainerProvider(connection, tracer);
            fileShareProvider = new FileShareProvider(context, connection, tracer);
        }

        public IArtifactProvider GetProvider(BuildArtifact buildArtifact)
        {
            string artifactType = buildArtifact.Resource.Type;
            if (PipelineArtifactConstants.PipelineArtifact.Equals(artifactType, StringComparison.CurrentCultureIgnoreCase))
            {
                return pipelineArtifactProvider;
            }
            else if (PipelineArtifactConstants.Container.Equals(artifactType, StringComparison.CurrentCultureIgnoreCase))
            {
                return fileContainerProvider;
            }
            else if (PipelineArtifactConstants.FileShareArtifact.Equals(artifactType, StringComparison.CurrentCultureIgnoreCase))
            {
                return fileShareProvider;
            }
            else
            {
                throw new InvalidOperationException($"{buildArtifact} is not of type PipelineArtifact, FileShare or BuildArtifact");
            }
        }
    }
}
