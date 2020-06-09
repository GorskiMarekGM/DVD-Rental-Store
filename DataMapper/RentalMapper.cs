using System;
using System.Collections.Generic;
using System.Configuration;
using Npgsql;

namespace DataMapper
{
    class RentalMapper : IMapper<Rental>
    {
        private static readonly string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["Rental"].ToString();
        private readonly Dictionary<int, Rental> _cache = new Dictionary<int, Rental>();

        public static RentalMapper Instance { get; } = new RentalMapper();
        // This is a singleton, so constructor is private
        private RentalMapper() { }

        public Rental GetByID(int id)
        {
            if (_cache.ContainsKey(id))
            {
                return _cache[id];
            }
            Rental rental = GetByIDFromDB(id);
            _cache.Add(rental.ID, rental);
            return rental;
        }

        private Rental GetByIDFromDB(int id)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM rentals WHERE rental_id = @ID", conn))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    NpgsqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();

                        return new Rental (id,(int)reader["copy_id"], (int)reader["client_id"], (DateTime)reader["date_of_rental"], (DateTime)reader["date_of_return"]);
                    }
                }
            }
            return null;
        }

        public void Save(Rental rental)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();
                // This is an UPSERT operation - if record doesn't exist in the database it is created, otherwise it is updated
                using (var command = new NpgsqlCommand("INSERT INTO rentals(copy_id, client_id, date_of_rental, date_of_return, rental_id) " +
                    "VALUES (@copyID, @clientID, @dateOfRental, @dateOfReturn, @ID) " +
                    "ON CONFLICT (rental_id) DO UPDATE " +
                    "SET copy_id = @copyID, client_id = @clientID, date_of_rental = @dateOfRental, date_of_return=@dateOfReturn", conn))
                {
                    command.Parameters.AddWithValue("@copyID", rental.CopyID);
                    command.Parameters.AddWithValue("@clientID", rental.ClientID);
                    command.Parameters.AddWithValue("@dateOfRental", rental.DateOfRental);
                    command.Parameters.AddWithValue("@dateOfReturn", rental.DateOfReturn);
                    command.Parameters.AddWithValue("@ID", rental.ID);
                    command.ExecuteNonQuery();
                }
            }
            _cache[rental.ID] = rental;
        }

        public void Delete(Rental t)
        {
            throw new NotImplementedException();
        }
    }
}
