<?php
header("Access-Control-Allow-Origin: *");
header("Content-Type: application/json; charset=UTF-8");

include 'db.php';

// Decode JSON payload
$input = json_decode(file_get_contents("php://input"), true);
$studentId = $input['StudentID'];
$classId = $input['ClassID'];
$date = $input['Date'];
$status = $input['Status'];

// Check if the attendance record already exists
$query = "SELECT * FROM Attendance WHERE StudentID = ? AND ClassID = ? AND Date = ?";
$stmt = $conn->prepare($query);
$stmt->bind_param("iis", $studentId, $classId, $date);
$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    // Update existing record
    $updateQuery = "UPDATE Attendance SET Status = ? WHERE StudentID = ? AND ClassID = ? AND Date = ?";
    $updateStmt = $conn->prepare($updateQuery);
    $updateStmt->bind_param("siis", $status, $studentId, $classId, $date);
    $updateStmt->execute();

    echo json_encode(["message" => "Attendance updated successfully"]);
} else {
    // Insert new record
    $insertQuery = "INSERT INTO Attendance (StudentID, ClassID, Date, Status) VALUES (?, ?, ?, ?)";
    $insertStmt = $conn->prepare($insertQuery);
    $insertStmt->bind_param("iiss", $studentId, $classId, $date, $status);
    $insertStmt->execute();

    echo json_encode(["message" => "Attendance recorded successfully"]);
}

$conn->close();
?>