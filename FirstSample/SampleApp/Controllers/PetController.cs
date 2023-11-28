using SampleApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace SampleApp.Controllers
{
	public class PetController : Controller
	{
		// GET: Pet
		public ActionResult Pets()
		{
			return View();
		}

		public ActionResult ViewAll()
		{
			if (Request.IsAjaxRequest())
			{
				// If the request is AJAX, return JSON data
				//var pets = GetAllPets().ToList();
				return Json(GetAllPets(), JsonRequestBehavior.AllowGet);
			}

			// Otherwise, return the view
			return View(GetAllPets());
		}

		IEnumerable<Pet> GetAllPets()
		{
			using (DBModelClass db = new DBModelClass())
			{
				return db.Pets.ToList<Pet>();
			}
		}

		public ActionResult AddOrEdit(int id = 0)
		{
			Pet pets = new Pet();

			// Check if has pet id to be edit and show on second tab or edit tab
			if (id != 0)
			{
				using (DBModelClass db = new DBModelClass())
				{
					pets = db.Pets.Where(x => x.PetID == id).FirstOrDefault<Pet>();
				}
			}
			return View(pets);
		}

		[HttpPost]

		public ActionResult AddOrEdit(Pet pets)
		{
			try
			{

				using (DBModelClass db = new DBModelClass())
				{
					if (pets.PetID == 0)
					{
						// Save the data with iamge
						string fileName = Path.GetFileNameWithoutExtension(pets.ImageUpload.FileName);
						string extension = Path.GetExtension(pets.ImageUpload.FileName);
						fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
						pets.PetImagePath = "~/AppFiles/Images/" + fileName;
						pets.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));

						// This is a new pet, add it to the database
						db.Pets.Add(pets);
						db.SaveChanges();
						//return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllPets()), message = "Pet added successfully!" }, JsonRequestBehavior.AllowGet);
						return Json(new { success = true, message = "Pet added successfully!" });

					}
					else
					{
						// This is an existing pet, update it in the database
						var existingPet = db.Pets.Find(pets.PetID);
						if (existingPet == null)
						{
							// Handle the case where the pet doesn't exist (optional)
							//return Json(new { success = false, message = "Pet not found for update." }, JsonRequestBehavior.AllowGet);
							return Json(new { success = false, message = "Pet not found for update." });
						}

						// Update the properties of the existing pet with the values from the input pet
						existingPet.PetName = pets.PetName;
						existingPet.PetAge = pets.PetAge;
						existingPet.PetGender = pets.PetGender;

						// Check if has new image
						if (pets.ImageUpload != null)
						{
							// Update the image if it's provided
							string fileName = Path.GetFileNameWithoutExtension(pets.ImageUpload.FileName);
							string extension = Path.GetExtension(pets.ImageUpload.FileName);
							fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
							existingPet.PetImagePath = "~/AppFiles/Images/" + fileName;
							pets.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
						}


						// Update the database
						db.Entry(existingPet).State = EntityState.Modified;
						db.SaveChanges();

						//return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllPets()), message = "Pet updated successfully!" }, JsonRequestBehavior.AllowGet);
						return Json(new { success = true, message = "Pet updated successfully!" });
					}
				}
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "An error occurred while saving the pet: " + ex.Message });
			}
		}



		public ActionResult Delete(int id)
		{
			using (DBModelClass db = new DBModelClass())
			{
				var pet = db.Pets.Find(id);
				if (pet == null)
				{
					return Json(new { success = false, message = "Pet not found for deletion." }, JsonRequestBehavior.AllowGet);
				}

				// Check if the pet has its own image or using the default image
				bool hasOwnImage = !string.IsNullOrEmpty(pet.PetImagePath) && !pet.PetImagePath.ToLower().Contains("default");
				if (hasOwnImage)
				{
					// Delete the image file from the directory
					DeleteImage(pet.PetImagePath);
				}
				

				// Remove the pet from the database
				db.Pets.Remove(pet);
				db.SaveChanges();

				//return Json(new { success = true, message = "Pet deleted successfully." }, JsonRequestBehavior.AllowGet);
				return Json(new { success = true, message = "Pet deleted successfully." });
			}
		}

		private void DeleteImage(string imagePath)
		{
			try
			{
				// Map the virtual path to the physical path
				string physicalPath = Server.MapPath(imagePath);

				// Check if the file exists before attempting to delete it
				if (System.IO.File.Exists(physicalPath))
				{
					// Check if the image is the default image
					if (IsDefaultImage(imagePath))
					{
						// If it's the default image, do not delete it
						return;
					}

					// Delete the file
					System.IO.File.Delete(physicalPath);
				}
				else
				{
					// If file not found
					Console.WriteLine($"Error deleting image: File not found - {imagePath}");

				}
			}
			catch (Exception ex)
			{
				// Handle any exceptions that may occur during file deletion
				// You might want to log the exception or take appropriate action
				Console.WriteLine($"Error deleting image: {ex.Message}");
			}
		}

		private bool IsDefaultImage(string imagePath)
		{
			// Define your default image path (adjust this based on your actual default image path)
			string defaultImagePath = "~/AppFiles/Images/default.jpg";

			// Compare the image path with the default image path
			return string.Equals(imagePath, defaultImagePath, StringComparison.OrdinalIgnoreCase);
		}



		

		//private void DeleteImage(string imagePath)
		//{
		//	try
		//	{
		//		// Map the virtual path to the physical path
		//		string physicalPath = Server.MapPath(imagePath);

		//		// Check if the file exists before attempting to delete it
		//		if (System.IO.File.Exists(physicalPath))
		//		{
		//			// Delete the file
		//			System.IO.File.Delete(physicalPath);
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		// Handle any exceptions that may occur during file deletion
		//		// You might want to log the exception or take appropriate action
		//		Console.WriteLine($"Error deleting image: {ex.Message}");
		//	}
		//}


	}
}