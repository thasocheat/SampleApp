using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace SampleApp.Models
{
	public class ModalDB
	{
		//declare connection string
		string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
		// Accessing the connection string


		//Return list of all Pets
		public List<ModalPets> ListAll()
		{
			List<ModalPets> lst = new List<ModalPets>();
			using (SqlConnection con = new SqlConnection(cs))
			{
				con.Open();
				SqlCommand com = new SqlCommand("SelectPets", con);
				com.CommandType = CommandType.StoredProcedure;
				SqlDataReader rdr = com.ExecuteReader();
				while (rdr.Read())
				{
					lst.Add(new ModalPets
					{
						PetID = Convert.ToInt32(rdr["PetID"]),
						PetName = rdr["PetName"].ToString(),
						PetAge = rdr["PetAge"].ToString(),
						PetGender = rdr["PetGender"].ToString(),
						PetImagePath = rdr["PetImagePath"].ToString(),
					});
				}
				return lst;
			}
		}

		public ModalPets GetbyID(int ID)
		{
			using (SqlConnection con = new SqlConnection(cs))
			{
				con.Open();
				SqlCommand com = new SqlCommand("SelectPetByID", con);
				com.CommandType = CommandType.StoredProcedure;
				com.Parameters.AddWithValue("@PetID", ID);

				SqlDataReader rdr = com.ExecuteReader();

				if (rdr.Read())
				{
					return new ModalPets
					{
						PetID = Convert.ToInt32(rdr["PetID"]),
						PetName = rdr["PetName"].ToString(),
						PetAge = rdr["PetAge"].ToString(),
						PetGender = rdr["PetGender"].ToString(),
						PetImagePath = rdr["PetImagePath"].ToString(),
					};
				}

				return null;
			}
		}

		//Method for Adding an Employee
		public int Add(ModalPets pets)
		{
			int i;
			using (SqlConnection con = new SqlConnection(cs))
			{
				con.Open();
				SqlCommand com = new SqlCommand("InsertUpdatePets", con);
				com.CommandType = CommandType.StoredProcedure;
				com.Parameters.AddWithValue("@PetID", pets.PetID);
				com.Parameters.AddWithValue("@PetName", pets.PetName);
				com.Parameters.AddWithValue("@PetAge", pets.PetAge);
				com.Parameters.AddWithValue("@PetGender", pets.PetGender);
				com.Parameters.AddWithValue("@PetImagePath", pets.PetImagePath);
				com.Parameters.AddWithValue("@Action", "Insert");
				i = com.ExecuteNonQuery();
			}
			return i;
		}

		//Method for Updating Employee record
		public int Update(ModalPets pets)
		{
			int i;
			using (SqlConnection con = new SqlConnection(cs))
			{
				con.Open();
				SqlCommand com = new SqlCommand("InsertUpdatePets", con);
				com.CommandType = CommandType.StoredProcedure;
				com.Parameters.AddWithValue("@PetID", pets.PetID);
				com.Parameters.AddWithValue("@PetName", pets.PetName);
				com.Parameters.AddWithValue("@PetAge", pets.PetAge);
				com.Parameters.AddWithValue("@PetGender", pets.PetGender);
				com.Parameters.AddWithValue("@PetImagePath", pets.PetImagePath);
				com.Parameters.AddWithValue("@Action", "Update");
				i = com.ExecuteNonQuery();
			}
			return i;
		}

		//Method for Deleting an Employee
		public int Delete(int ID)
		{
			int i;
			using (SqlConnection con = new SqlConnection(cs))
			{
				con.Open();
				SqlCommand com = new SqlCommand("DeletePets", con);
				com.CommandType = CommandType.StoredProcedure;
				com.Parameters.AddWithValue("@PetID", ID);
				i = com.ExecuteNonQuery();
			}
			return i;
		}
	}
}