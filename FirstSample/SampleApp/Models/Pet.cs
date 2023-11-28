//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SampleApp.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Web;

    public partial class Pet
    {
        public int PetID { get; set; }

		[DisplayName("Pet Name")]
        [Required(ErrorMessage = "Please, put your pet name.")]
		public string PetName { get; set; }

		[DisplayName("Pet Age")]
		[Required(ErrorMessage = "Please, put your pet age.")]
		public string PetAge { get; set; }

		[DisplayName("Pet Gender")]
		[Required(ErrorMessage = "Please, put your pet gender.")]
		public string PetGender { get; set; }

        [DisplayName("Pet Image")]
		//[Required(ErrorMessage = "Please, put your pet photo.")]
		public string PetImagePath { get; set; }


        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }


        public Pet()
        {
            PetImagePath = "~/AppFiles/Images/default.png";
        }
    }
}