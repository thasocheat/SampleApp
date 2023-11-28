using SampleApp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleApp.Controllers
{
	public class HomeController : Controller
	{
		ModalDB petDB = new ModalDB();
		public ActionResult Index()
		{
			return View();
		}

		public JsonResult List()
		{
			return Json(petDB.ListAll(), JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Add(ModalPets pets)
		{
			try
			{
				// If has any file to upload?
				if(Request.Files.Count > 0)
				{
					var file = Request.Files[0];
					// If the file not null and has file than
					if(file != null && file.ContentLength > 0)
					{
						// Save the upload file to a folder
						string fileName = Path.GetFileNameWithoutExtension(file.FileName);
						string extension = Path.GetExtension(file.FileName);
						fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
						pets.PetImagePath = "~/AppFiles/Images/" + fileName;
						file.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
					}
				}
				else
				{
					// If no file for uploaded, set a default image path
					pets.PetImagePath = "~/AppFiles/Images/default.png";
				}
				var result = petDB.Add(pets);

				//return Json(petDB.Add(pets), JsonRequestBehavior.AllowGet);
				return Json(new { success = true, message = "Pet added successfully!" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}
		public JsonResult GetbyID(int ID)
		{
			var ModalPets = petDB.ListAll().Find(x => x.PetID.Equals(ID));
			return Json(ModalPets, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Update(ModalPets pets)
		{
			try
			{
				// If there's any file to upload for update
				if (Request.Files.Count > 0)
				{
					var file = Request.Files[0];

					// If the file is not null and has content
					if (file != null && file.ContentLength > 0)
					{
						// Delete the existing file if it exists
						if (!string.IsNullOrEmpty(pets.PetImagePath))
						{
							string oldImagePath = Path.Combine(Server.MapPath(pets.PetImagePath));
							if (System.IO.File.Exists(oldImagePath))
							{
								System.IO.File.Delete(oldImagePath);
							}
						}

						// Save the upload file to a folder
						string fileName = Path.GetFileNameWithoutExtension(file.FileName);
						string extension = Path.GetExtension(file.FileName);
						fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
						pets.PetImagePath = "/AppFiles/Images/" + fileName;
						file.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
					}
				}

				var result = petDB.Update(pets);

				// Perform the rest of the update logic
				// ...

				return Json(new { success = true, message = "Pet updated successfully!" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}



		[HttpPost]
		public JsonResult Delete(int ID)
		{
			try
			{
				var pet = petDB.GetbyID(ID);

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

				// Now delete the pet record
				var result = petDB.Delete(ID);
				return Json(new { success = true, message = "Pet deleted successfully!" });
				//return Json(petDB.Delete(ID), JsonRequestBehavior.AllowGet);
			}
			catch(Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
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

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}