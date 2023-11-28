using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SampleApp.Models
{
	public class ModalPets
	{
		public int PetID { get; set; } 

		public string PetName { get; set; } 

		public string PetAge { get; set; } 

		public string PetGender { get; set; } 

		public string PetImagePath { get; set; }

		[NotMapped]
		public HttpPostedFileBase ImageUpload { get; set; }

		//public void PetDefaultImage()
		//{
		//	PetImagePath = "~/AppFiles/Images/default.png";
		//}
	}
}