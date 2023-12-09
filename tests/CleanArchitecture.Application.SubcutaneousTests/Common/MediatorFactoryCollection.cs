namespace CleanArchitecture.Application.SubcutaneousTests.Common;

[CollectionDefinition(CollectionName)]
public class MediatorFactoryCollection : ICollectionFixture<MediatorFactory>
{
    public const string CollectionName = "MediatorFactoryCollection";
}