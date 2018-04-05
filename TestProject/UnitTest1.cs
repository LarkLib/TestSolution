using System;
using TestConsoleApp;
using Xunit;


namespace TestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var source = new Person() { Name = "name",Age = 19, PhoneNumber = "1234567890",Address = "address 1"};
            var info = source.GetPersonInfo();
            Assert.Equal("Name=name,Age=19,PhoneNumber=1234567890",info);
        }
    }
}