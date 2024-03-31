// Function to calculate the total booking amount
function calculateTotalAmount() {
    var roomIdSelect = document.getElementById("RoomId");
    var selectedRoomOption = roomIdSelect.options[roomIdSelect.selectedIndex];
    var selectedRoomPrice = parseFloat(selectedRoomOption.getAttribute("data-price"));
    var checkInDate = new Date(document.getElementById("CheckInDate").value);
    var checkOutDate = new Date(document.getElementById("CheckOutDate").value);

    // Calculate the booking duration in milliseconds
    var bookingDuration = checkOutDate - checkInDate;

    // Convert booking duration to days
    var bookingDays = bookingDuration / (1000 * 60 * 60 * 24);

    // Calculate the total booking amount
    var totalAmount = selectedRoomPrice * bookingDays;

    // Set the total amount in the TotalAmount input field
    document.getElementById("TotalAmount").value = totalAmount.toFixed(2);
}

// Attach change event handler to room selection dropdown
document.getElementById("RoomId").addEventListener("change", function () {
    calculateTotalAmount();
});

// Attach change event handler to check-in and check-out date inputs
document.getElementById("CheckInDate").addEventListener("change", function () {
    calculateTotalAmount();
});

document.getElementById("CheckOutDate").addEventListener("change", function () {
    calculateTotalAmount();
});

// Initial calculation when the page loads
calculateTotalAmount();