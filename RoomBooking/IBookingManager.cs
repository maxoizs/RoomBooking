using System;
using System.Collections.Generic;

namespace RoomBooking
{
	public interface IBookingManager
	{
		/// <summary>
		/// Return true if there is no booking for the given room on the date,
		/// otherwise false
		/// </summary>
		bool IsRoomAvailable(int room, DateTime date);

		/// <summary>
		/// Add a booking for the given guest in the given room on the given
		/// date.If the room is not available, throw a suitable Exception.
		/// </summary>
		void AddBooking(string guest, int room, DateTime date);

		/// <summary>
		/// Return a list of all the available room numbers for the given date
		/// </summary>
		IEnumerable<int> GetAvailableRooms(DateTime date);
	}
}
