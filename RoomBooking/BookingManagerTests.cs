using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace RoomBooking
{
	public class BookingManagerTests
	{
		private const int _validRoom = 101;
		private readonly List<int> _rooms = new() { _validRoom, 12, 3, 5 };
		private readonly DateTime _today = new(2012, 3, 28);

		[Test]
		public void GivenRoomAvailable_ShouldReturnTrue()
		{
			IBookingManager bm = new BookingManager(_rooms);

			bm.IsRoomAvailable(_validRoom, _today).Should().BeTrue();
		}

		[Test]
		public void GivenRoomNotExists_ShouldReturnFalse()
		{
			IBookingManager bm = new BookingManager(_rooms);

			bm.IsRoomAvailable(0, _today).Should().BeFalse();
		}

		[Test]
		public void GivenRoomAvailable_ShouldBookIt()
		{
			IBookingManager bm = new BookingManager(_rooms);

			var act = () => bm.AddBooking("Patel", _validRoom, _today);

			act.Should().NotThrow();
		}

		[Test]
		public void GivenRoomBooked_ShouldReturnFalse()
		{
			IBookingManager bm = new BookingManager(_rooms);

			bm.AddBooking("Patel", _validRoom, _today);

			bm.IsRoomAvailable(_validRoom, _today).Should().BeFalse();
		}

		[Test]
		public void GivenRoomBooked_ShouldThrowWhenBook()
		{
			IBookingManager bm = new BookingManager(_rooms);
			bm.AddBooking("Patel", _validRoom, _today);

			var act = () => bm.AddBooking("Li", _validRoom, _today);

			act.Should().Throw<InvalidOperationException>();
		}

		[Test]
		public void GivenRoomBooked_ShouldNotReturnItInAvailabilties()
		{
			IBookingManager bm = new BookingManager(_rooms);
			bm.AddBooking("Patel", _validRoom, _today);

			var availableRooms = bm.GetAvailableRooms(_today);

			availableRooms.Should().NotContain(_validRoom);
			availableRooms.Should().BeEquivalentTo(new List<int> { 12, 3, 5 });
		}
	}
}
