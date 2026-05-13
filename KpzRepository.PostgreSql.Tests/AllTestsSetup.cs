using Config.Net;

namespace KpzRepository.PostgreSql.Tests;

[SetUpFixture]
public class AllTestsSetup
{
    public static IAppSettings? Settings { get; private set; }

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        Settings = new ConfigurationBuilder<IAppSettings>()
            .UseJsonFile("appsettings.json")
            .Build();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {

    }
}
