﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xigadee;

namespace Test.Xigadee
{
    [TestClass]
    public class ChannelRewrite
    {
        [TestMethod]
        public void Rewrite1()
        {
            try
            {
                var ms = new MicroservicePipeline();
                CommandInitiator init;
                ms
                    .AddChannelIncoming("deadletter")
                    .AddChannelIncoming("freddy")
                        .AttachCommand<RewriteCommandVerifyFail>()
                        .AttachMessageRedirectRule(
                            new ServiceMessageHeader("freddy", "one", "two")
                           , new ServiceMessageHeader("findlay", "three", "four")
                           )
                    .AddChannelIncoming("findlay")
                        .AttachCommand<RewriteCommandVerifySuccess>()
                    .AddChannelOutgoing("response")
                    ;

                ms.Start();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public class RewriteCommandVerifySuccess: CommandBase
    {

        [CommandContract("findlay", "three", "four")]
        [return: PayloadOut]
        public string Verify([PayloadIn]string inData)
        {
            return "fail";
        }
    }
    public class RewriteCommandVerifyFail: CommandBase
    {

        [CommandContract("freddy", "one", "two")]
        [return: PayloadOut]
        public string Verify([PayloadIn]string inData)
        {
            return "pass";
        }
    }
}
