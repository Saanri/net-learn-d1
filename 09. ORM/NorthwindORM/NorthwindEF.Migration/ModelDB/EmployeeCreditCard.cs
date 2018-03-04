namespace NorthwindEF.Migration.ModelDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EmployeeCreditCard
    {
   /*     [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmployeeCreditCard()
        {
            Employees = new HashSet<Employee>();
        }*/

        [Key]
        public int CreditCardsID { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        public string CardHolderName { get; set; }

        public int? EmployeeID { get; set; }

        public virtual Employee Employee { get; set; }
/*
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees { get; set; }*/
    }
}