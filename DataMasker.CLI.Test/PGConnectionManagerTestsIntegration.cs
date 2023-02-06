using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMasker.CLI.Test
{
    [TestFixture]
    public class PGConnectionManagerTestsIntegration
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetTablesNameAsync_should_throw_arguement_exception_when_connectionstring_is_null()
        {
            //Arrange
            var connectionString = "";
            var connectionManager = new PGConnectionManager(connectionString);

            //Act
            var invalidOperationException = Assert.ThrowsAsync<InvalidOperationException>(async () => await connectionManager.GetTableNamesAsync());

            //Assert
            Assert.That(invalidOperationException.Message, Does.Contain("The ConnectionString property has not been initialized."));
        }

        [Test]
        public void GetTablesNameAsync_should_throw_postgresexception_with_sqlstate_28P01_when_database_password_is_invalid()
        {
            //Arrange
            var connectionString = "User ID=postgres;Password=invalidpassword;Host=localhost;Port=5432;Database=test_db;Pooling=true;";
            var connectionManager = new PGConnectionManager(connectionString);

            //Act
            var postgresException = Assert.ThrowsAsync<Npgsql.PostgresException>(async () => await connectionManager.GetTableNamesAsync());

            //Assert
            Assert.That(postgresException.Message, Does.Contain("password authentication failed"));
            Assert.That(postgresException.SqlState, Is.EqualTo("28P01"));
        }

        [Test]
        public void GetTablesNameAsync_should_throw_postgresexception_with_sqlstate_28P01_when_database_userid_is_invalid()
        {
            //Arrange
            var connectionString = "User ID=ivaliduserid;Password=postgres;Host=localhost;Port=5432;Database=test_db;Pooling=true;";
            var connectionManager = new PGConnectionManager(connectionString);

            //Act
            var postgresException = Assert.ThrowsAsync<Npgsql.PostgresException>(async () => await connectionManager.GetTableNamesAsync());

            //Assert
            Assert.That(postgresException.Message, Does.Contain("password authentication failed"));
            Assert.That(postgresException.SqlState, Is.EqualTo("28P01"));
        }

        [Test]
        public void GetTablesNameAsync_should_throw_postgresexception_with_sqlstate_is_null_when_database_port_is_invalid()
        {
            //Arrange
            var connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=invalidport;Database=test_db;Pooling=true;";
            var connectionManager = new PGConnectionManager(connectionString);

            //Act
            var argumentException = Assert.ThrowsAsync<ArgumentException>(async () => await connectionManager.GetTableNamesAsync());

            //Assert
            //
            Assert.That(argumentException.Message, Does.Contain("Couldn't set port (Parameter 'port')"));
        }


        [TearDown]
        public void TearDown()
        {
        }
    }
}
