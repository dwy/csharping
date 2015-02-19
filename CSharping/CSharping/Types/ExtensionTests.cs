using NUnit.Framework;

namespace CSharping.Types
{
    [TestFixture]
    public class ExtensionTests
    {
        [Test]
        public void ExtensionMethod()
        {
            var employee = new Employee(1);

            decimal salary = employee.GetSalary(1000);

            Assert.AreEqual(2000, salary);
        }
    }

    public static class EmployeeExtenstion
    {
        public static decimal GetSalary(this Employee employee, decimal bonus)
        {
            return employee.Type * 1000 + bonus;
        }
    }

    public class Employee
    {
        private readonly int _type;

        public Employee(int type)
        {
            _type = type;
        }

        public int Type
        {
            get { return _type; }
        }
    }
}