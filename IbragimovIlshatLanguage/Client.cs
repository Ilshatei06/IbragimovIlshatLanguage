//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IbragimovIlshatLanguage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            this.ClientService = new HashSet<ClientService>();
            this.Tag = new HashSet<Tag>();
        }
    
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string FullFIO
        {
            get
            {
                if (!string.IsNullOrEmpty(Patronymic))
                    return FirstName + " " + LastName + " " + Patronymic;
                else
                    return FirstName + " " + LastName;
            }
        }
        public string GenderCode { get; set; }
        public string Phone { get; set; }
        public string PhotoPath { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }

        public string Email { get; set; }
        public System.DateTime RegistrationDate { get; set; }

        public string RegistrationDateString
        {
            get
            {
                if (this.RegistrationDate != null)
                {
                    return RegistrationDate.ToShortDateString();
                }
                else return "";
            }
        }

        public string BirthdayString
        {
            get
            {
                if (this.Birthday != null)
                {
                    return Birthday.Value.ToShortDateString();
                }
                else return "";
            }
        }

        public int VisitCount
        {
            get
            {
                var datelist = ClientService.Where(p => p.ClientID == this.ID).ToList();
                return datelist.Count;
            }
        }
        public string LastVisit
        {
            get
            {
                var current = ClientService.OrderBy(p => p.StartTime).LastOrDefault();

                if (current != null)
                {
                    return current.StartTime.ToShortDateString();
                }
                return "";
            }
        }

        public DateTime LastVisitDate
        {
            get
            {
                var cur = ClientService.OrderBy(p => p.StartTime).LastOrDefault();

                if (cur != null)
                    return cur.StartTime;
                else
                    return DateTime.MinValue;

            }
        }



        public virtual Gender Gender { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public string GenderName
        {
            get
            {
                return Gender.Name;
            }
        }
       

        public virtual ICollection<ClientService> ClientService { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tag> Tag { get; set; }
    }
}
