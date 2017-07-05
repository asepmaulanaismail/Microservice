﻿#region Copyright
// Copyright Hitachi Consulting
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xigadee;
using System.Threading.Tasks;

namespace Test.Xigadee
{
    [TestClass]
    public partial class PipelineTest3
    {

        [TestMethod]
        public void Pipeline3()
        {
            try
            {
                var pipeline = new MicroservicePipeline("TestPipeline");
                var destination = ServiceMessageHeader.FromKey("internalIn/frankie/benny");

                ICommandInitiator init;
                DebugMemoryDataCollector collector;

                int signalChange = 0;

                pipeline
                    .AddDebugMemoryDataCollector(out collector)
                    .AdjustPolicyTaskManager((t, c) =>
                    {
                        t.ConcurrentRequestsMin = 1;
                        t.ConcurrentRequestsMax = 4;
                    })
                    .AddChannelIncoming("internalIn", internalOnly: true)
                        .AttachCommand((rq,rs,c) => {
                            return Task.FromResult(0);
                        }, destination)
                        .AttachICommandInitiator(out init)
                        .Revert()
                    .AddChannelOutgoing("internalOut", internalOnly: true)
                        .Revert();

                pipeline.Start();

                //var rs = init.Process("internalIn", "frankie", "benny", "Hello").

                pipeline.Stop();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }
    }
}
