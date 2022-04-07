using FluentAssertions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RoomBooking
{
	public class BookingManager : IBookingManager
	{
		private ConcurrentDictionary<int, List<Booking>> _availabilities = new ConcurrentDictionary<int, List<Booking>>();
		public BookingManager(List<int> rooms)
		{
			rooms.ForEach((room) => _availabilities.TryAdd(room, new List<Booking>()));
		}

		public void AddBooking(string guest, int room, DateTime date)
		{
			if (!IsRoomAvailable(room, date))
			{
				throw new InvalidOperationException("Room not avialable");
			}

			var booking = new Booking() { Guest = guest, Date = date.Date };

			_availabilities
			.AddOrUpdate(room, new List<Booking> { booking }, (room, exitingBookings) =>
		 {
			 exitingBookings.Add(booking);
			 return exitingBookings;
		 });
		}

		public IEnumerable<int> GetAvailableRooms(DateTime date)
		{
			var days = _availabilities
				.Where(availability => 
					!availability.Value.Any(booking =>
								booking.Date.Date.Equals(date.Date))
				)
				.Select(availability => availability.Key);
			return days;
		}

		public bool IsRoomAvailable(int room, DateTime date)
		{
			if (!_availabilities.ContainsKey(room))
			{
				return false;
			}
			var bookings = _availabilities[room];
			return !bookings.Any(b => b.Date.Date.Equals(date.Date));
		}
	}
}
