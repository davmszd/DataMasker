namespace DataMasker.CLI.Test
{
    [TestFixture]
    public class PGConnectionManagerTests
    {
        [Test]
        public void PGConnectionManagerTests_should_receive_a_string_in_constructor()
        {
            //Arrange
            var connectionString = "";

            //Act
            var connectionManager = new PGConnectionManager(connectionString);

            //Assert
            Assert.That(connectionManager, Is.Not.Null);
        }

        [Test]
        public void PGConnectionManager_should_be_instance_of_IConnectionManager()
        {
            //Arrange
            var connectionString = "";

            //Act
            var connectionManager = new PGConnectionManager(connectionString);

            //Assert
            Assert.That(connectionManager, Is.InstanceOf<IConnectionManager>());
        }
    }
}
