using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitApp.Models
{
    [MetadataType(typeof(GroupMetadata))]
    public class Group
    {
        public Group()
        {
            this.Users = new HashSet<User>();
            this.Challenges = new HashSet<Challenge>();

        }

        public int Id { get; set; }
        public int MembersCount { get; set; }
        public string GroupName { get; set; }
        public int AdminId { get; set; }
        public string GroupType { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Challenge> Challenges { get; set; }

    }

    public class GroupMetadata
    {
        [Display(Name = "Ilość użytkowników")]
        public int MembersCount { get; set; }

        [Display(Name = "Nazwa grupy")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Podanie nazwy grupy jest wymagane")]
        public string GroupName { get; set; }

        [Display(Name = "Typ grupy")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Podanie typu grupy jest wymagane")]
        public string GroupType { get; set; }
    }


}