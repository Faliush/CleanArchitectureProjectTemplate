
using System.Threading.Channels;

namespace Application.Abstractions.Events;

internal sealed class InMemmoryMessageQueue
{
    private readonly Channel<IIntegrationEvent> _channel = Channel.CreateUnbounded<IIntegrationEvent>();

    public ChannelWriter<IIntegrationEvent> Writer => _channel.Writer;

    public ChannelReader<IIntegrationEvent> Reader => _channel.Reader;
}


