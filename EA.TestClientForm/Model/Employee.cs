using System;

namespace EA.TestClientForm.Model
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Department { get; set; }

        public DateTimeOffset DateTime { get; set; }

        public string Photo { get; set; }
    }
}