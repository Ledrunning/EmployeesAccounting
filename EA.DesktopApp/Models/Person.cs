using System;

namespace EA.DesktopApp.Models
{
    /// <summary>
    ///     Model for Web Api client
    ///     Need to point to System.ComponentModel.DataAnnotations;
    /// </summary>
    public class Person
    {
        /// <summary>
        ///     Person Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Person name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Person Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Where person works
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        ///     Added time
        /// </summary>
        public DateTimeOffset DateTime { get; set; }

        /// <summary>
        ///     Image path
        /// </summary>
        public string Photo { get; set; }
    }
}