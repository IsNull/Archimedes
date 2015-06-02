using Archimedes.DI.AOP;

namespace Archimedes.Patterns.Tests.ContainerTest.Primary
{
    [Primary(typeof(IServiceCandidate))]
    [Service]
    public class ServiceCandidate2 : ServiceCandidate1
    {

    }
}
