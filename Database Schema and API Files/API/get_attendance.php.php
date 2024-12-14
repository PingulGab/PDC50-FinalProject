<?php
header("Access-Control-Allow-Origin: *");
header("Content-Type: application/json; charset=UTF-8");

include 'db.php';

// Retrieve query parameters
$studentId = $_GET['student_id'];
$classId = $_GET['class_id'];
$date = $_GET['date'];

// Query for attendance record
$query = "SELECT * FROM Attendance WHERE StudentID = ? AND ClassID = ? AND Date = ?";
$stmt = $conn->prepare($query);
$stmt->bind_param("iis", $studentId, $classId, $date);
$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    $attendance = $result->fetch_assoc();
    echo json_encode($attendance);
} else {
    echo json_encode(["message" => "No attendance record found"]);
}

$conn->close();
?>