//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace techwales.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;
    using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

    public partial class USER
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        
        [NotMapped]
        [Required(ErrorMessage = "Confirm Password required")]
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public Nullable<bool> Isactive { get; set; }
        public Nullable<bool> Isremembered { get; set; }
        //[Required(ErrorMessage = "Type is required.")]
        [Required]
        public string Type { get; set; }
        
        public string REG_STATUS { get; set; }
    }
}
