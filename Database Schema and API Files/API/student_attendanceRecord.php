<?php
include 'db.php';

header('Content-Type: application/json');

// Check for valid student ID in request
if (!isset($_GET['student_id'])) {
    echo json_encode(['error' => 'Student ID is required']);
    exit;
}

$student_id = intval($_GET['student_id']);

// Fetch attendance records
$query = "SELECT DATE_FORMAT(date, '%Y-%m-%d') as date,
                 CASE status
                     WHEN 1 THEN 'Present'
                     WHEN 2 THEN 'Absent'
                     WHEN 3 THEN 'Excused'
                     ELSE 'Unknown'
                 END as status
          FROM attendance
          WHERE StudentID = ?";

$stmt = $conn->prepare($query);

if ($stmt === false) {
    echo json_encode(['error' => 'Failed to prepare the statement']);
    exit;
}

// Bind the parameter and execute
$stmt->bind_param('i', $student_id);
$stmt->execute();

// Fetch results
$result = $stmt->get_result();
$attendanceRecords = [];
while ($row = $result->fetch_assoc()) {
    $attendanceRecords[] = $row;
}

// Close the statement and connection
$stmt->close();
$conn->close();

// Output the JSON response
echo json_encode($attendanceRecords);
