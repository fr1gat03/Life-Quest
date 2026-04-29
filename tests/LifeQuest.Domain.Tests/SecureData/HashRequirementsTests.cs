namespace LifeQuest.Domain.Tests.SecureData;

using LifeQuest.Domain.SecureData;

public class HashRequirementsTests
{
    private HashRequirements _hashRequirements;

    [SetUp]
    public void Setup()
    {
        _hashRequirements = new HashRequirements();
    }

    [Test]
    public void AddAccount_DuplicateLogin_ReturnsFalse()
    {
        _hashRequirements.AddAccount("user1", "password123");

        bool result = _hashRequirements.AddAccount("user1", "otherpassword");

        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHash_TwoAccounts_ReturnsDifferentHashes()
    {
        _hashRequirements.AddAccount("user1", "password123");
        _hashRequirements.AddAccount("user2", "password123");

        string hash1 = _hashRequirements.GetHash("user1");
        string hash2 = _hashRequirements.GetHash("user2");

        Assert.That(hash1, Is.Not.EqualTo(hash2));
    }

    [Test]
    public void GetHash_NonExistingLogin_ThrowsKeyNotFoundException()
    {
        Assert.Throws<KeyNotFoundException>(() => _hashRequirements.GetHash("ghost"));
    }
}