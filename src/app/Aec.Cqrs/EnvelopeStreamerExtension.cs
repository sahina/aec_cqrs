using System;

namespace Aec.Cqrs
{
    public static class EnvelopeStreamerExtension
    {
        public static byte[] SaveEnvelopeData(this IEnvelopeStreamer streamer, object message,
                                              Action<EnvelopeBuilder> build = null)
        {
            var builder = new EnvelopeBuilder(Guid.NewGuid().ToString());
            builder.AddItem(message);
            if (null != build)
            {
                build(builder);
            }
            return streamer.SaveEnvelopeData(builder.Build());
        }
    }
}