using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Display(Name = "Worker")]
    public class Worker
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Project")]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [Display(Name = "Project")]
        public Project Project { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        public Job Job { get; set; }

        //public IList<Skill> Skills { get; set; }

        [DisplayName("Years Of Experience")]
        public int YearsOfExperience { get; set; }
    }

    public enum Job
    {
        [Display(Name = "Not Specified")]
        NotSpecified,
        ProjectLeader,
        Developper,
        QATester, 
    }

    //public enum Skill
    //{
    //    [Display(Name = "Not Specified")]
    //    NotSpecified,
    //    CSharp,
    //    SCRUM,
    //    Cplusplus,
    //    C,
    //    Jenkins,
    //    Azure,
    //}
}
