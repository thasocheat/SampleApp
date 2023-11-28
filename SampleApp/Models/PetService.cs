using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SampleApp.Models
{
	public class PetService
	{
		private ModalDB petDB;
		private string imagesFolderPath; // Path where pet images are stored

		public PetService(string imagesFolderPath)
		{
			petDB = new ModalDB();
			this.imagesFolderPath = imagesFolderPath;
		}

		public void AddOrUpdatePet(ModalPets pets)
		{
			if (pets == null)
				throw new ArgumentNullException(nameof(pets));

			// Handle file upload logic
			HandleFileUpload(pets);

			// Perform add or update in the database
			if (pets.PetID == 0)
				petDB.Add(pets);
			else
				petDB.Update(pets);
		}

		public ModalPets GetPetByID(int ID)
		{
			return petDB.GetbyID(ID);
		}

		public void DeletePet(int ID)
		{
			var pet = petDB.GetbyID(ID);

			// Delete associated image file
			DeletePetImage(pet);

			// Delete the pet record
			petDB.Delete(ID);
		}

		private void HandleFileUpload(ModalPets pets)
		{
			if (pets.ImageUpload != null && pets.ImageUpload.ContentLength > 0)
			{
				string fileName = Path.GetFileNameWithoutExtension(pets.ImageUpload.FileName);
				string extension = Path.GetExtension(pets.ImageUpload.FileName);
				fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
				pets.PetImagePath = "~/AppFiles/Images/" + fileName;
				pets.ImageUpload.SaveAs(Path.Combine(imagesFolderPath, fileName));
			}
			else
			{
				// If no file uploaded, set a default image path
				pets.PetImagePath = "~/AppFiles/Images/default.png";
			}
		}

		private void DeletePetImage(ModalPets pet)
		{
			if (!string.IsNullOrEmpty(pet?.PetImagePath))
			{
				string imagePath = Path.Combine(imagesFolderPath, pet.PetImagePath);
				if (System.IO.File.Exists(imagePath))
				{
					System.IO.File.Delete(imagePath);
				}
			}
		}
	}
}